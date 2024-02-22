using Context;
using System;
using System.Collections.Generic;
using Entity;
using System.Linq;
using System.Data.Entity;

namespace Repository
{
    public class ProductRepository : IRepository<Product>, IDisposable
    {
        private readonly EmpresaLSTPContext _context;
        public ProductRepository(){
            this._context = new EmpresaLSTPContext();
        }
        public void Create(Product e)
        {
            this._context.Products.Add(e);
            this._context.SaveChanges();
        }

        public void Delete(int id)
        {
            Product toRemove = this._context.Products.Where(x=> x.Id == id).FirstOrDefault();
            this._context.Products.Remove(toRemove);
            this._context.SaveChanges();
        }

        public Product Retrive(int id)
        {
            return this._context.Products.Where(x => x.Id == id).FirstOrDefault();
        }

        public List<Product> Retrive(Product e)
        {
            return this._context.Products.Where(x => 
                x.Id == e.Id && 
                x.Name == e.Name && 
                x.Description == e.Description &&
                x.Price == e.Price &&
                x.Stock == e.Stock).ToList();
        }
        public List<Product> RetriveAll()
        {
            return this._context.Products.ToList();
        }
        public void Update(Product e)
        {
            this._context.Products.Attach(e);
            this._context.Entry(e).State = EntityState.Modified;
            this._context.SaveChanges();
        }
        public void Dispose()
        {
            this._context.Dispose();
        }
    }
}
