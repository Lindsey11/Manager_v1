using System.Data.Entity;
using System.Linq;
using Invoice.Model.DataAceess.Interface;
using Invoice.Model.DataContext;

namespace Invoice.Model.DataAceess
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class , new()
    {
        private  InvoiceDataContext invoiceDbContext;
        private  DbSet<T> _innerDbSet;
        public BaseRepository()
        {
            invoiceDbContext = new InvoiceDataContext();
            _innerDbSet = invoiceDbContext.Set<T>();
        }
        public void Add(T model)
        {
            _innerDbSet.Add(model);
            Save();
        }
        public void Update(T model)
        {
            invoiceDbContext.Entry(model).State = EntityState.Modified;
            invoiceDbContext.Configuration.ValidateOnSaveEnabled = false;
             Save();
            invoiceDbContext.Configuration.ValidateOnSaveEnabled = true;
        }
        public void Remove(int id)
        {
            var entity = Find(id);
            _innerDbSet.Remove(entity);
            Save();
        }
        public IQueryable<T> All()
        {
            return invoiceDbContext.Set<T>();
        }
        public T Find(int id)
        {
            return invoiceDbContext.Set<T>().Find(id);
        }

        private void Save()
        {
            invoiceDbContext.SaveChanges();
        }
    }
}
