using Microsoft.EntityFrameworkCore;

namespace RestWithASPNET.Model.Context
{
    public class MySQLContext : DbContext
    {
        public MySQLContext(DbContextOptions<MySQLContext> options): base(options)
        {

        }

        protected MySQLContext()
        {
        }

        public DbSet<Person> Persons { get; set; }
    }
}
