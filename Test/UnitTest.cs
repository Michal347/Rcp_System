using Microsoft.EntityFrameworkCore;
using Moq;
using FluentAssertions;
using RCP_Sys.Db;
using RCP_Sys.Models;
using RCP_Sys.Repository;
using NUnit.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    public class UnitTest
    {
     
        [Fact]
        public void Will_call_save_changes()
        {

            var mockContext = new Mock<RcpDbContext>();
            var userService = new UserService();

            userService.Save();


            mockContext.Verify(x => x.SaveChanges());

        }
        [Fact]
        public void willAdd()
        {
            var user = new UserModel()
            {
                Username = "Test",
                Password = "test",
                Surname = "Test",
                Name = "Test",
                DateTimeJoined = DateTime.Now,
                Email = "Test@gmail.com",
                IsUserAdmin = false,

            };

            var mockContext = new Mock<RcpDbContext>();
            var userService = new UserService();

            userService.Create(user);


            mockContext.Verify(x => x.Add(user));

        }

        [Fact]
        public void GetUSername()
        {
            var user = new UserModel()
            {
                Username = "Test",
                Password = "test",
                Surname = "Test",
                Name = "Test",
                DateTimeJoined = DateTime.Now,
                Email = "Test@gmail.com",
                IsUserAdmin = true,

            };

            string username = user.Username;

            var mockContext = new Mock<RcpDbContext>();
            var userService = new UserService();

            userService.GetUserModels(username);


            mockContext.Verify();

        }

    }
    }
