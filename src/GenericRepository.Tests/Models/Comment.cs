using System;
using System.Collections.Generic;
using IHomer.GenericRepository.Attributes;
using IHomer.GenericRepository.Interfaces;

namespace IHomer.GenericRepository.Tests.Models
{
    public partial class Comment : IEntity<long>
    {
        public long Id { get; set; }
        public long PostId { get; set; }
        public int UserId { get; set; }
        public DateTime Created { get; set; }
        public string Body { get; set; }
        [Parent]
        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
