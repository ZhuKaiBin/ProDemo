using Moq;
using Xunit;

namespace MoqMock
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }


    public interface IUserService
    {
        User GetUserById(int id);
    }

    public class User
    {
        public int Id
        {
            get;
            set;
        }
        public string Name { get; set; }
    }

    public class UserController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public User GetUser(int id)
        {
            return _userService.GetUserById(id);
        }
    }

    //https://www.leavescn.com/Articles/Content/3415
    //https://www.cnblogs.com/zjoch/p/6565757.html
    public class UserControllerTest
    {
        [Fact]
        public void GetUser_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var user = new User { Id = 1, Name = "John Doe" };
            mockUserService.Setup(service => service.GetUserById(1)).Returns(user);

            var userController = new UserController(mockUserService.Object);

            // Act
            var result = userController.GetUser(1);

            // Assert
            Assert.NotNull(result); // 修正这个断言
            Assert.Equal(1, result.Id);
            Assert.Equal("John Doe", result.Name);

            // Verify that the GetUserById method was called exactly once with the parameter 1
            mockUserService.Verify(service => service.GetUserById(1), Times.Once);
        }
    }

}
