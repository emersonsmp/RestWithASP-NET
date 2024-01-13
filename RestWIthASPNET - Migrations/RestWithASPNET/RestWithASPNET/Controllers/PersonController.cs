using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNET.Business;
using RestWithASPNET.Model;

namespace RestWithASPNET.Controllers
{
    //[ApiVersion("1")]
    //[ApiController]
    //[Route("api/[controller]/v{version:apiVersion}")]
    [ApiController]
    [Route("api/person/v1")]
    public class PersonController : ControllerBase
    {

        private readonly ILogger<PersonController> _logger;
        private IPersonBusiness _personBusiness;

        public PersonController(ILogger<PersonController> logger, IPersonBusiness personBusiness)
        {
            _logger = logger;
            _personBusiness = personBusiness;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_personBusiness.FindAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var person = _personBusiness.FindById(id);
            if(person == null) 
            {
                NotFound();
            }

            return Ok(person);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Person person)
        {
            if (person == null)
            {
                return BadRequest();
            }

            return Ok(_personBusiness.Create(person));
        }

        [HttpPut]
        public IActionResult Put([FromBody] Person person)
        {
            if (person == null)
            {
                return BadRequest();
            }

            return Ok(_personBusiness.Update(person));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _personBusiness.Delete(id);
            return NoContent();
        }
    }


    [ApiController]
    [Route("api/person/v2")]
    //[ApiVersion("2")]
    //[ApiController]
    //[Route("api/person/v{version:apiVersion}")]
    public class PersonControllerV2 : ControllerBase
    {

        private readonly ILogger<PersonControllerV2> _logger;
        private IPersonBusiness _personBusiness;

        public PersonControllerV2(ILogger<PersonControllerV2> logger, IPersonBusiness personBusiness)
        {
            _logger = logger;
            _personBusiness = personBusiness;
        }

        [HttpGet]
        public IActionResult Get()
        {
            //exemplo: poderia ser uma nova api com alteração deu uma propriedade no response
            //que poderia criar aplicaçoes antiigas
            return Ok(_personBusiness.FindAll());
        }
    }
}
