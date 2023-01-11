
using Autofac.Extras.Moq;
using Bogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RCP_Sys.Db;
using RCP_Sys.Models;
using RCP_Sys.Repository;
using RCP_Sys.ViewModels;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace RcpSystem.Test
{
    [TestClass]
    public class UserServiceTest
    {

        List<UserModel> usertest;
        IUserService userService;

        private List<UserModel> GenerateUser(int count)
        {
            var users = new Faker<UserModel>()
               .RuleFor(c => c.Email, f => "test@email.com")
               .RuleFor(c => c.Username, f => "Test")
               .RuleFor(c => c.Name, f => "Test")
               .RuleFor(c => c.Password, f => "Test")
               .RuleFor(c => c.DateTimeJoined, f => DateTime.Now)
               .RuleFor(c => c.IsUserAdmin, f => false)
               .RuleFor(c => c.Surname, f => "Test");

            return users.Generate(count);

        }

        [Fact]
        public async Task Init()
        {
            var usertest= GenerateUser(1);
            var data= usertest.AsQueryable();
            var mockDbContext = new Mock<RcpDbContext>();
            var mockDbSet = new Mock<DbSet<UserModel>>();
            mockDbSet.As<IQueryable<UserModel>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<UserModel>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<UserModel>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<UserModel>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockDbContext.Setup(x => x.Users).Returns(mockDbSet.Object);

            mockDbContext.Setup(x => x.SaveChanges()).Returns(1);

            userService = new UserService(mockDbContext.Object);
            var result = await userService.Create(usertest.FirstOrDefault());
        }

        [Fact]
        public async Task add_UserModel()
        {

            var usertest = GenerateUser(1);
            var result = await userService.Create(usertest.FirstOrDefault());

            result.Should().BeTrue();
        }


        [Fact]
        public void Will_call_save_changes()
        {

            var mockContext = new Mock<RcpDbContext>();
            var userService = new UserService(mockContext.Object);

            userService.Save();


            mockContext.Verify(x => x.SaveChanges());

        }
   
       
    }
}