using System.Linq;

namespace Invoice.Model.DataAceess.Interface
{
    public interface IBaseRepository<T> where T : class , new()
    {
        void Add(T model);
        void Update(T model);
        void Remove(int id);
        IQueryable<T> All();
        T Find(int id);
    }
}
