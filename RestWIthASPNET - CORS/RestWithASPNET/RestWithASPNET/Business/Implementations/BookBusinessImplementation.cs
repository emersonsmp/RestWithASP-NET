using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using RestWithASPNET.Data.Converter.Implementations;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Model;
using RestWithASPNET.Model.Context;
using RestWithASPNET.Repository;
using RestWithASPNET.Repository.Base;
using System;

namespace RestWithASPNET.Business.Implementations
{
    public class BookBusinessImplementation : IBookBusiness
    {
        private readonly IRepository<Book> _repository;
        private readonly BoookConverter converter;

        public BookBusinessImplementation(IRepository<Book> repository) { 
            _repository = repository;
            converter = new BoookConverter();
        }

        public List<BookVO> FindAll()
        {
            var books = _repository.FindAll();
            return converter.Parse(books);
        }

        public BookVO FindById(long id)
        {
            return converter.Parse(_repository.FindById(id));
        }

        public BookVO Create(BookVO bookVO)
        {
            var book = converter.Parse(bookVO);
            return converter.Parse(_repository.Create(book));
        }

        public BookVO Update(BookVO bookVO)
        {
            var book = converter.Parse(bookVO);
            return converter.Parse(_repository.Update(book));
        }

        public void Delete(long id)
        {
            _repository.Delete(id);
        }
    }
}
