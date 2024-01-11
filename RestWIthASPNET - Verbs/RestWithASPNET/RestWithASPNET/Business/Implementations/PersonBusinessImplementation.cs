using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using RestWithASPNET.Model;
using RestWithASPNET.Model.Context;
using RestWithASPNET.Repository;
using System;

namespace RestWithASPNET.Business.Implementations
{
    public class PersonBusinessImplementation : IPersonBusiness
    {
        private readonly IPersonRepository _repository;
        public PersonBusinessImplementation(IPersonRepository repository) { 
            _repository = repository;
        }

        public List<Person> FindAll()
        {
            return _repository.FindAll();
        }

        public Person FindById(long id)
        {
            return _repository.FindById(id);
        }

        public Person Create(Person person)
        {
            //pode incluir regras de negocio
            //exemplo: so crie se for maior de idade
            return _repository.Create(person);
        }

        public Person Update(Person person)
        {       
            return _repository.Update(person);
        }

        public void Delete(long id)
        {
            _repository.Delete(id);
        }
    }
}
