using Microsoft.AspNetCore.Identity;
using RestWithASPNET.Data.Converter.Contract;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Model;

namespace RestWithASPNET.Data.Converter.Implementations
{
    public class PersonConverter : IParser<PersonVO, Person>, IParser<Person, PersonVO>
    {
        public Person Parse(PersonVO origin)
        {
            if (origin == null) 
                return null;

            return new Person
            {
                Id = origin.Id,
                Name = origin.Name,
                LastName = origin.LastName,
                Gender = origin.Gender,
                Address = origin.Address
            };
        }

        public PersonVO Parse(Person origin)
        {
            if (origin == null) 
                return null;

            return new PersonVO
            {
                Id = origin.Id,
                Name = origin.Name,
                LastName = origin.LastName,
                Gender = origin.Gender,
                Address = origin.Address
            };
        }

        public List<Person> Parse(List<PersonVO> origin)
        {
            if (origin == null) 
                return null;

            return origin.Select(x => Parse(x)).ToList();
        }

        public List<PersonVO> Parse(List<Person> origin)
        {
            if (origin == null)
                return null;

            return origin.Select(x => Parse(x)).ToList();
        }
    }
}
