using Microsoft.AspNetCore.Identity;
using RestWithASPNET.Data.Converter.Contract;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Model;

namespace RestWithASPNET.Data.Converter.Implementations
{
    public class BoookConverter : IParser<BookVO, Book>, IParser<Book, BookVO>
    {
        public Book Parse(BookVO origin)
        {
            if (origin == null) 
                return null;

            return new Book
            {
                Id = origin.Id,
                Author = origin.Author,
                Lauch_date = origin.Lauch_date,
                Title = origin.Title,
                Price = origin.Price
            };
        }

        public BookVO Parse(Book origin)
        {
            if (origin == null) 
                return null;

            return new BookVO
            {
                Id = origin.Id,
                Author = origin.Author,
                Lauch_date = origin.Lauch_date,
                Title = origin.Title,
                Price = origin.Price
            };
        }

        public List<Book> Parse(List<BookVO> origin)
        {
            if (origin == null) 
                return null;

            return origin.Select(x => Parse(x)).ToList();
        }

        public List<BookVO> Parse(List<Book> origin)
        {
            if (origin == null)
                return null;

            return origin.Select(x => Parse(x)).ToList();
        }
    }
}
