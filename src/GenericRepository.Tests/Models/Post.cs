using System;
using System.Collections.Generic;
using IHomer.GenericRepository.Attributes;
using IHomer.GenericRepository.Interfaces;

namespace IHomer.GenericRepository.Tests.Models
{
    public partial class Post : IEntity<long>
    {
        public Post()
        {
            Comments = new List<Comment>();
        }

        public long Id { get; set; }
        public int UserId { get; set; }
        public DateTime Created { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        [Parent]
        public virtual User User { get; set; }
    }
}
