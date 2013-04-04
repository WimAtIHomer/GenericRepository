using IHomer.GenericRepository.Interfaces;
using System.Data.Entity;

namespace IHomer.GenericRepository
{
    public class UnitOfWork<TC> : IUnitOfWork<TC>
        where TC : DbContext, new()
    {
        private readonly TC _entities = new TC();

        public TC Entities
        {
            get { return _entities; }
        }

        public int SaveChanges()
        {
            return _entities.SaveChanges();
        }
    }
}
