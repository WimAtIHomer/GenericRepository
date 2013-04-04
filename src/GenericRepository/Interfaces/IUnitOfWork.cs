using System.Data.Entity;

namespace IHomer.GenericRepository.Interfaces
{
    public interface IUnitOfWork<out T> : ISaveChanges
        where T : DbContext
    {
        T Entities { get; }
    }
}
