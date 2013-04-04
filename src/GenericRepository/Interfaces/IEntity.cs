using System;

namespace IHomer.GenericRepository.Interfaces
{
    public interface IEntity<TId> where TId : IComparable<TId>
    {
        TId Id { get; set; }
    }
}
