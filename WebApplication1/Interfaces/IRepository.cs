using WebApplication1.Devices;

namespace WebApplication1.Interfaces
{
    public interface IRepository<T> where T : Device
    {
        IEnumerable<T> GetAll();
        T? GetById(Guid id);
        void Add(T entity);
        void Delete(Guid id);
    }

}
