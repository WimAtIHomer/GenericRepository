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
    public class RoleRepository : GenericRepository<Role, GenericRepositoryTestContext, byte>
    {
        public RoleRepository(IUnitOfWork<GenericRepositoryTestContext> context) : base(context) { }

    }
}
