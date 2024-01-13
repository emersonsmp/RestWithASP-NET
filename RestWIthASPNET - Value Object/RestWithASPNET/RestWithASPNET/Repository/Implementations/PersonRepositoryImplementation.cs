using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using RestWithASPNET.Model;
using RestWithASPNET.Model.Context;
using RestWithASPNET.Repository.Base;
using System;

namespace RestWithASPNET.Repository.Implementations
{
    public class PersonRepositoryImplementation : IRepository<Person>
    {
        private MySQLContext _context;
        public PersonRepositoryImplementation(MySQLContext context) { 
            _context = context;
        }

        public List<Person> FindAll()
        {
            return _context.Persons.ToList();
        }

        public Person FindById(long id)
        {
            return _context.Persons.SingleOrDefault(x => x.Id == id);
        }

        public Person Create(Person person)
        {
            try
            {
                _context.Add(person);
                _context.SaveChanges();
            }
            catch(Exception e)
            {
                throw;
            }
            
            return person;
        }

        public Person Update(Person person)
        {
            if (!Exists(person.Id)) return null;

            var result = _context.Persons.SingleOrDefault(x => x.Id == person.Id);

            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(person);
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    throw;
                }

            }

            return person;
        }

        public void Delete(long id)
        {
            try
            {
                var result = _context.Persons.SingleOrDefault(x => x.Id == id);

                if (result != null)
                {
                    _context.Persons.Remove(result);
                    _context.SaveChanges();
                }
                
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public bool Exists(long id)
        {
            return _context.Persons.Any(p => p.Id == id);
        }
    }
}
