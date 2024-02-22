using System.Collections.Generic;

namespace Repository
{
    public interface IRepository<T>
    {
        List<T> RetriveAll ();
        T Retrive(int id);
        List<T> Retrive(T e);
        void Update(T e);
        void Delete(int id);
        void Create(T e);
    }
}
