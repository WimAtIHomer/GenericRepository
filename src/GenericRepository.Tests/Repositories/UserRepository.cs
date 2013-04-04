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
    public class UserRepository : GenericRepository<User, GenericRepositoryTestContext, int>
    {
        public UserRepository(IUnitOfWork<GenericRepositoryTestContext> context) : base(context) { }

        public override IQueryable<User> GetAll()
        {
            return Context.Entities.Users.Include(u => u.Roles);
        }
    }
}
