using System;
using System.Data.Entity.Validation;
using System.Linq;
using IHomer.GenericRepository.Tests.Models;
using IHomer.GenericRepository.Tests.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IHomer.GenericRepository.Tests
{
    [TestClass]
    public class UniqueTests
    {
        private static TestContext _testContext;

        [ClassInitialize]
        public static void ClassSetup(TestContext testContext)
        {
            _testContext = testContext;
            using (var context = new GenericRepositoryTestContext())
            {
                context.Database.Initialize(false);
            }
        }

        [TestMethod]
        public void InsertUserCheckUnique()
        {
            var unitOfWork = new UnitOfWork<GenericRepositoryTestContext>();
            var user = new User
            {
                Email = "wim.pool@ihomer.nl",
                Name = "Wim Pool"
            };
            var userRepo = new UserRepository(unitOfWork);
            userRepo.Add(user);
            try
            {
                userRepo.SaveChanges();
            }
            catch (DbEntityValidationException validationException)
            {
                foreach (var error in validationException.EntityValidationErrors)
                {
                    Assert.IsTrue(error.ValidationErrors.Count == 1);
                    Assert.AreEqual(error.ValidationErrors.First().ErrorMessage, "NotUnique");
                    Assert.AreEqual(error.ValidationErrors.First().PropertyName, "Email");
                }
            }
            var users = userRepo.FindBy(u => u.Email == "wim.pool@ihomer.nl");
            Assert.IsTrue(users.Count() == 1);
        }
    }
}
