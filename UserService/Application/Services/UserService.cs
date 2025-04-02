using Logging.Interfaces;
using UserService.Application.Constants;
using UserService.Application.Helpers;
using UserService.Application.Interfaces;
using UserService.Domain.DTOs;
using UserService.Domain.Entities;
using UserService.Infrastructure.Interfaces;

namespace UserService.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ILoggerService _logger;

        public UserService(IUserRepository userRepository, IConfiguration configuration, ILoggerService logger)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<UserResponseDto> RegisterAsync(UserRegisterDto registerDto)
        {
            _logger.LogInfo($"[Register] Kayıt işlemi başlatıldı. Email: {registerDto.Email}");

            var existingUser = await _userRepository.GetByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                _logger.LogWarning($"[Register] Kayıt başarısız. Email zaten kayıtlı: {registerDto.Email}");
                throw new Exception(ErrorMessages.EmailAlreadyExists);
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                Role = registerDto.Role,
                PasswordHash = PasswordHelper.HashPassword(registerDto.Password)
            };

            await _userRepository.CreateAsync(user);

            _logger.LogInfo($"[Register] {SuccessMessages.UserCreated}: {registerDto.Email}");

            return new UserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            };
        }
        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            _logger.LogInfo($"[Login] Giriş denemesi başladı. Email: {loginDto.Email}");

            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null || !PasswordHelper.VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                _logger.LogWarning($"[Login] Giriş başarısız. Hatalı email veya şifre: {loginDto.Email}");
                throw new Exception(ErrorMessages.InvalidCredentials);
            }

            var token = JwtTokenGenerator.GenerateToken(user, _configuration);

            _logger.LogInfo($"[Login] {SuccessMessages.LoginSuccessful}. Email: {loginDto.Email}");

            return new LoginResponseDto
            {
                Token = token,
                Email = user.Email,
                Role = user.Role,
                ExpiresAt = DateTime.UtcNow.AddHours(2)
            };
        }
        public async Task<List<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return users.Select(user => new UserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            }).ToList();
        }
        public async Task<UserResponseDto> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new Exception(ErrorMessages.UserNotFound);

            return new UserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            };
        }
        public async Task<UserResponseDto> UpdateUserAsync(Guid id, UserUpdateDto updateDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new Exception(ErrorMessages.UserNotFound);

            user.FullName = updateDto.FullName;
            user.Email = updateDto.Email;
            user.Role = updateDto.Role;

            await _userRepository.UpdateAsync(user);

            _logger.LogInfo($"[Update] {SuccessMessages.UserUpdated}: {user.Email}");

            return new UserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            };
        }
        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var result = await _userRepository.DeleteAsync(id);

            if (result)
                _logger.LogInfo($"[Delete] {SuccessMessages.UserDeleted}: ID {id}");
            else
                _logger.LogWarning($"[Delete] Kullanıcı silinemedi. ID: {id}");

            return result;
        }
    }
}
