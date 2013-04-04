using System;
using System.Collections.Generic;
using IHomer.GenericRepository.Attributes;
using IHomer.GenericRepository.Interfaces;

namespace IHomer.GenericRepository.Tests.Models
{
    public partial class User : IEntity<int>
    {
        public User()
        {
            Comments = new List<Comment>();
            Posts = new List<Post>();
            Roles = new List<Role>();
        }

        public int Id { get; set; }
        [Unique]
        public string Email { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}
