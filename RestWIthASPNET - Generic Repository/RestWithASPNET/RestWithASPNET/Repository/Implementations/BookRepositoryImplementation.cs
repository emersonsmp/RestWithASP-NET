using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using RestWithASPNET.Model;
using RestWithASPNET.Model.Context;
using RestWithASPNET.Repository.Base;
using System;

namespace RestWithASPNET.Repository.Implementations
{
    public class BookRepositoryImplementation : IRepository<Book>
    {
        private MySQLContext _context;
        public BookRepositoryImplementation(MySQLContext context) { 
            _context = context;
        }

        public Book Create(Book book)
        {
            try
            {
                _context.Add(book);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }

            return book;
        }

        public void Delete(long id)
        {
            var result = _context.Books.SingleOrDefault(x => x.Id == id);

            if (result != null)
            {
                _context.Books.Remove(result);
                _context.SaveChanges();
            }
        }

        public bool Exists(long id)
        {
            return _context.Books.Any(x => x.Id == id);
        }

        public List<Book> FindAll()
        {
            return _context.Books.ToList();
        }

        public Book FindById(long id)
        {
            return _context.Books.SingleOrDefault(x => x.Id == id);
        }

        public Book Update(Book book)
        {
            if (Exists(book.Id))
            {
                _context.Books.Update(book);
            }
            else
            {
                return null;
            }

            return book;
        }
    }
}
