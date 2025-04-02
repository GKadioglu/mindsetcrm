using Xunit;
using Moq;
using UserService.Domain.Entities;
using UserService.Domain.DTOs;
using UserService.Infrastructure.Interfaces;
using Logging.Interfaces;
using UserService.Application.Constants;
using UserService.Application.Helpers;

namespace UserService.Tests.Services
{
    public class UserServiceTests
    {
        [Fact]
        public async Task LoginAsync_ValidCredentials_ReturnsToken()
        {
            var userEmail = "test@example.com";
            var password = "123456";

            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetByEmailAsync(userEmail)).ReturnsAsync(new User
            {
                Id = Guid.NewGuid(),
                Email = userEmail,
                PasswordHash = PasswordHelper.HashPassword(password),
                Role = "Admin"
            });

            var inMemorySettings = new Dictionary<string, string>
            {
                {"Jwt:Key", "TestJwtKey123456789-TestKey-For-UnitTests!"},
                {"Jwt:Issuer", "TestIssuer"},
                {"Jwt:Audience", "TestAudience"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings!)
                .Build();

            var mockLogger = new Mock<ILoggerService>();

            var userService = new UserService.Application.Services.UserService(
                mockRepo.Object,
                configuration,
                mockLogger.Object
            );

            var loginDto = new LoginDto { Email = userEmail, Password = password };

            var result = await userService.LoginAsync(loginDto);

            Assert.NotNull(result);
            Assert.Equal(userEmail, result.Email);
            Assert.False(string.IsNullOrEmpty(result.Token));
        }

        [Fact]
        public async Task LoginAsync_InvalidCredentials_ThrowsException()
        {
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetByEmailAsync("wrong@example.com")).ReturnsAsync((User?)null);

            var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Jwt:Key", "TestJwtKey123456789-TestKey-For-UnitTests!" },
                { "Jwt:Issuer", "TestIssuer" },
                { "Jwt:Audience", "TestAudience" }
            }).Build();

            var mockLogger = new Mock<ILoggerService>();
            var userService = new UserService.Application.Services.UserService(mockRepo.Object, config, mockLogger.Object);

            var loginDto = new LoginDto
            {
                Email = "wrong@example.com",
                Password = "wrongpass"
            };

            var exception = await Assert.ThrowsAsync<Exception>(() => userService.LoginAsync(loginDto));
            Assert.Equal(ErrorMessages.InvalidCredentials, exception.Message);
        }

        [Fact]
        public async Task RegisterAsync_ValidData_ReturnsUserResponse()
        {
            var registerDto = new UserRegisterDto
            {
                FullName = "Test User",
                Email = "newuser@example.com",
                Password = "123456",
                Role = "User"
            };

            var createdUser = new User
            {
                Id = Guid.NewGuid(),
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                PasswordHash = PasswordHelper.HashPassword(registerDto.Password),
                Role = registerDto.Role
            };

            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetByEmailAsync(registerDto.Email)).ReturnsAsync((User?)null);
            mockRepo.Setup(r => r.CreateAsync(It.IsAny<User>())).Returns(Task.FromResult(createdUser));

            var inMemorySettings = new Dictionary<string, string>
            {
                {"Jwt:Key", "TestJwtKey123456789-TestKey-For-UnitTests!"},
                {"Jwt:Issuer", "TestIssuer"},
                {"Jwt:Audience", "TestAudience"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings!)
                .Build();

            var mockLogger = new Mock<ILoggerService>();

            var userService = new UserService.Application.Services.UserService(
                mockRepo.Object,
                configuration,
                mockLogger.Object
            );

            var result = await userService.RegisterAsync(registerDto);

            Assert.NotNull(result);
            Assert.Equal(registerDto.Email, result.Email);
            Assert.Equal(registerDto.FullName, result.FullName);
            Assert.Equal(registerDto.Role, result.Role);
        }

        [Fact]
        public async Task RegisterAsync_ExistingUser_ThrowsException()
        {
            var email = "duplicate@example.com";

            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync(new User { Email = email });

            var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>()).Build();
            var mockLogger = new Mock<ILoggerService>();

            var userService = new UserService.Application.Services.UserService(mockRepo.Object, config, mockLogger.Object);

            var registerDto = new UserRegisterDto
            {
                FullName = "Dup User",
                Email = email,
                Password = "pass",
                Role = "User"
            };

            var ex = await Assert.ThrowsAsync<Exception>(() => userService.RegisterAsync(registerDto));
            Assert.Equal(ErrorMessages.EmailAlreadyExists, ex.Message);
        }
    }
}
