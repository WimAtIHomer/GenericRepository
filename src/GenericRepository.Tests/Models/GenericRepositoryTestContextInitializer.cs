using System;
using System.Data.Entity;

namespace IHomer.GenericRepository.Tests.Models
{
    public class GenericRepositoryTestContextInitializer : DropCreateDatabaseAlways<GenericRepositoryTestContext>
    {
        protected override void Seed(GenericRepositoryTestContext context)
        {
            var adminRole = new Role { Name = "Admin" };
            context.Roles.Add(adminRole);
            var bloggerRole = new Role { Name = "Blogger" };
            context.Roles.Add(bloggerRole);

            var user = new User { Email = "wim.pool@ihomer.nl", Name = "Wim Pool" };
            context.Users.Add(user);
            adminRole.Users.Add(user);
            bloggerRole.Users.Add(user);

            var user2 = new User { Email = "wim@ihomer.nl", Name = "Wim2" };
            bloggerRole.Users.Add(user2);

            var post = new Post
                {
                    Body = "Generic Repository is a great start for your repository classes",
                    Created = DateTime.Now,
                    Title = "My first post",
                    User = user
                };
            context.Posts.Add(post);
            var comment = new Comment { Body = "What a great post!", Created = DateTime.Now, Post = post, User = user2 };
            context.Comments.Add(comment);
            var comment2 = new Comment { Body = "Thanx!", Created = DateTime.Now, Post = post, User = user };
            context.Comments.Add(comment2);
            context.SaveChanges();
        }
    }
}