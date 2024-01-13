using RestWithASPNET.Data.VO;
using RestWithASPNET.Model;

namespace RestWithASPNET.Business
{
    public interface IBookBusiness
    {
        BookVO Create(BookVO book);
        BookVO FindById(long Id);
        List<BookVO> FindAll();
        BookVO Update(BookVO book);
        void Delete(long Id);
    }
}
