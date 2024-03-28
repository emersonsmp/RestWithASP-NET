using RestWithASPNET.data.VO;
using RestWithASPNET.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestWithASPNET.Repository
{
    public interface IUserRepository
    {
        User? ValidateCreentials(UserVO user);
        User? ValidateCreentials(string userName);
        bool RevokeToken(string userName);
        User? RefreshUserInfo(User user);
    }
}
