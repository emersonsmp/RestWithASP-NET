using RestWithASPNET.Model;

namespace RestWithASPNET.Business
{
    public interface IBookBusiness
    {
        Book Create(Book book);
        Book FindById(long Id);
        List<Book> FindAll();
        Book Update(Book book);
        void Delete(long Id);
    }
}
