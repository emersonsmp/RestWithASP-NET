using Microsoft.IdentityModel.Tokens;
using RestWithASPNET.Configurations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RestWithASPNET.Sevices.Implementations
{
    public class TokenService : ITokenService
    {
        private TokenConfiguration _Configuration;

        public TokenService(TokenConfiguration Configuration)
        {
            _Configuration = Configuration;
        }

        //claims -> são os payloads / dados enviados (claims: reservados, publicos e privados)
        public string GenerateAcessToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration.Secret));
            var _signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var Options = new JwtSecurityToken(
                issuer: _Configuration.Issuer,
                audience: _Configuration.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_Configuration.Minutes),
                signingCredentials: _signinCredentials
                );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(Options);

            return tokenString;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration.Secret)),
                ValidateLifetime = true
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            ClaimsPrincipal principal;

            try
            {
                principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

            }catch(SecurityTokenExpiredException e)
            {
                throw new SecurityTokenException("Expired Token");
            }

            var jwtSeurityToken = securityToken as JwtSecurityToken;

            if (jwtSeurityToken == null || !jwtSeurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,StringComparison.InvariantCulture)) 
                throw new SecurityTokenException("Invalid Token");

            return principal;
        }
    }
}
