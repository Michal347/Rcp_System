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
            var userService = new UserService(mockContext.Object);

            userService.Save();


            mockContext.Verify();

        }
        [Fact]
        public void will_Add()
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
            var userService = new UserService(mockContext.Object);

            userService.Create(user);


            mockContext.Verify();

        }

        //[Fact]
        //public void Will_GetUSername()
        //{
        //    //var user = new UserModel()
        //    //{
        //    //    Username = "Test",
        //    //    Password = "test",
        //    //    Surname = "Test",
        //    //    Name = "Test",
        //    //    DateTimeJoined = DateTime.Now,
        //    //    Email = "Test@gmail.com",
        //    //    IsUserAdmin = true,

        //    //};

        //    string username= string.Empty;/* = user.Username;*/

        //    var mockContext = new Mock<RcpDbContext>();
        //    var userService = new UserService(mockContext.Object);

        //    userService.GetUserModels(username);


        //    mockContext.Verify();

        }

    }
    
