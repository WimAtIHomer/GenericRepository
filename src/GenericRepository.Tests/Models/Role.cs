using System;
using System.Collections.Generic;
using IHomer.GenericRepository.Attributes;
using IHomer.GenericRepository.Interfaces;

namespace IHomer.GenericRepository.Tests.Models
{
    public class Role : IEntity<byte>
    {
        public Role()
        {
            Users = new List<User>();
        }

        public byte Id { get; set; }
        [Unique]
        public string Name { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
