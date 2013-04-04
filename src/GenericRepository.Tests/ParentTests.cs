using System;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using IHomer.GenericRepository.Tests.Models;
using IHomer.GenericRepository.Tests.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IHomer.GenericRepository.Tests
{
    [TestClass]
    public class ParentTests
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

        /// <summary>
        /// User is not the parent of Comment, so remove gives exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RemoveCommentFromUserException()
        {
            var unitOfWork = new UnitOfWork<GenericRepositoryTestContext>();
            var userRepo = new UserRepository(unitOfWork);
            var user = userRepo.GetFirst(u => u.Email == "wim@ihomer.nl");
            var comment = user.Comments.FirstOrDefault();
            user.Comments.Remove(comment);

            userRepo.SaveChanges();
        }


        /// <summary>
        /// Post is the parent of Comment, so remove should now work
        /// </summary>
        [TestMethod]
        public void RemoveCommentFromPost()
        {
            var unitOfWork = new UnitOfWork<GenericRepositoryTestContext>();

            Assert.IsTrue(unitOfWork.Entities.Comments.Count() == 2);
            
            var userRepo = new UserRepository(unitOfWork);
            var user = userRepo.GetFirst(u => u.Email == "wim.pool@ihomer.nl");
            var post = user.Posts.FirstOrDefault();
            Debug.Assert(post != null, "post != null");
            var comment = post.Comments.FirstOrDefault(c => c.User == user);
            post.Comments.Remove(comment);
            userRepo.SaveChanges();

            Assert.IsTrue(unitOfWork.Entities.Comments.Count() == 1);
        }
    }
}
