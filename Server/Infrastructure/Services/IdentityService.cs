using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Auth;
using Application.Enums;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IDateTimeService _dateTimeService;
        private readonly IEmailService _emailService;
        private readonly IMaintenanceService _maintenanceService;
        private readonly ApplicationDbContext _dbContext;

        public IdentityService(SignInManager<User> signInManager,
                               UserManager<User> userManager,
                               IConfiguration configuration,
                               IDateTimeService dateTimeService,
                               ApplicationDbContext applicationDbContext,
                               IEmailService emailService,
                               IMaintenanceService maintenanceService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _dateTimeService = dateTimeService;
            _dbContext = applicationDbContext;
            _emailService = emailService;
            _maintenanceService = maintenanceService;
        }

        public async Task<Result> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
                return Result.Failure("Invalid username or password");

            if (!user.EmailConfirmed)
                return Result.Failure("E-mail not confirmed");

            var role = _dbContext.UserRoles.Where(x => x.UserId == user.Id).FirstOrDefault();
            if (role.RoleId == Convert.ToInt32(UserRole.RegularUser))
                return Result.Failure("Invalid user role");

            var claims = GetClaims(user);
            var token = GenerateJwtForUser(user, claims);

            //var maintenanceResult = await _maintenanceService.CheckExpirationDate();
            //if (!maintenanceResult.Success)
            //    throw new Exception("Unable to login because maintenace service return an exception.");

            //var clearResult = await _maintenanceService.ClearCheckIns();
            //if (!clearResult.Success)
            //    throw new Exception("Unable to login because maintenace service return an exception.");

            return Result.Successful(token);
        }

        public async Task<Result> LoginApp(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
                return Result.Failure("Invalid username or password");

            if (!user.EmailConfirmed)
                return Result.Failure("E-mail not confirmed");

            var role = _dbContext.UserRoles.Where(x => x.UserId == user.Id).FirstOrDefault();
            if (role.RoleId != Convert.ToInt32(UserRole.RegularUser))
                return Result.Failure("Invalid user role");

            var claims = GetClaims(user);
            var token = GenerateJwtForUser(user, claims);

            return Result.Successful(token);
        }

        public async Task<RegisterResult> Register(string email, string password, string firstName, string lastName, string address)
        {
            if (await _userManager.FindByEmailAsync(email) != null)
                return RegisterResult.Failure("User with given E-mail already exist");

            var user = new User
            {
                Email = email.ToLower(),
                UserName = email,
                FirstName = firstName,
                LastName = lastName,
                Address = address
            };

            var registerResult = await _userManager.CreateAsync(user, password);

            if (!registerResult.Succeeded)
                throw IdentityException.RegisterException(registerResult.Errors.Select(e => e.Description));

            await _emailService.SendConfirmationEmailAsync(user.Email, "token");

            return RegisterResult.Successful(user.Id);
        }

        private static List<Claim> GetClaims(User user)
        {
            var claims = new List<Claim>();

            if (user != null)
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.Email, user.Email));
            }

            return claims;
        }

        private string GenerateJwtForUser(User user, List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expireDate = _dateTimeService.Now.AddHours(Convert.ToInt32(_configuration["JWT:ExpireInHours"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: expireDate,
                claims: claims,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Result> ConfirmEmail(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return Result.Failure("There was an error while processing confirm email the request");

            var emailConfirmed = await _userManager.ConfirmEmailAsync(user, token);

            if (emailConfirmed.Succeeded)
                return Result.Successful();
            else
                return Result.Failure("There was error during confirmation");
        }

        public async Task<Result> ResetPassword(string email, string token, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return Result.Failure("There was an error while processing reset password");

            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, token, password);

            if (resetPasswordResult.Succeeded)
                return Result.Successful();
            else
                return Result.Failure("There was error during password reset request");
        }

        public async Task<Result> GenerateTokenForIdentityPurpose(string email, TokenPurpose purpose)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return Result.Failure("There was an error while processing token request");

            string token = string.Empty;

            switch (purpose)
            {
                case TokenPurpose.ConfirmEmail:
                    token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    break;

                case TokenPurpose.ResetPassword:
                    token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    break;
            }

            return Result.Successful(token);
        }
    }
}