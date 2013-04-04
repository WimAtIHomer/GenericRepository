using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using IHomer.GenericRepository;
using IHomer.GenericRepository.Interfaces;
using IHomer.GenericRepository.Tests.Models;

namespace IHomer.GenericRepository.Tests.Repositories
{
    public class PostRepository : GenericRepository<Post, GenericRepositoryTestContext, long>
    {
        public PostRepository(IUnitOfWork<GenericRepositoryTestContext> context) : base(context) { }

        public override IQueryable<Post> GetAll()
        {
            return Context.Entities.Posts.Include(p => p.Comments);
        }
    }
}