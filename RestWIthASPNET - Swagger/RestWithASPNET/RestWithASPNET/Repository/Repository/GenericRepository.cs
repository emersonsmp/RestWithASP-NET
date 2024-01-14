using Microsoft.EntityFrameworkCore;
using RestWithASPNET.Model;
using RestWithASPNET.Model.Base;
using RestWithASPNET.Model.Context;
using RestWithASPNET.Repository.Base;

namespace RestWithASPNET.Repository.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        private MySQLContext _context;
        private DbSet<T> dataset;
        public GenericRepository(MySQLContext context)
        {
            _context = context;
            dataset = _context.Set<T>();
        }


        public List<T> FindAll()
        {
            return dataset.ToList();
        }

        public T FindById(long id)
        {
            return dataset.SingleOrDefault(x => x.Id == id);
        }

        public T Create(T item)
        {
            try
            {
                dataset.Add(item);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw;
            }

            return item;
        }

        public T Update(T item)
        {
            if (!Exists(item.Id)) return null;

            var result = dataset.SingleOrDefault(x => x.Id == item.Id);

            if (result != null)
            {
                try
                {
                    dataset.Entry(result).CurrentValues.SetValues(item);
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    throw;
                }

            }

            return item;
        }

        public void Delete(long id)
        {
            try
            {
                var result = dataset.SingleOrDefault(x => x.Id == id);

                if (result != null)
                {
                    dataset.Remove(result);
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
            return dataset.Any(p => p.Id == id);
        }
    }
}
