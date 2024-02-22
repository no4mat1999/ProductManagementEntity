using Entity;
using System.Data.Entity;

namespace Context
{
    public class EmpresaLSTPContext:DbContext
    {
        public EmpresaLSTPContext() : base("name=EmpresaDbConnection"){}

        public virtual DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("Products");
        }
    }
}
