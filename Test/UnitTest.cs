using Microsoft.EntityFrameworkCore;
using Moq;
using RCP_Sys.Db;
using RCP_Sys.Models;
using RCP_Sys.Repository;

namespace Test
{
    public class UnitTest
    {
        private IUserService userservice;
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