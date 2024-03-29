﻿using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Auth;
using Application.Common.Models.BaseResult;
using Application.Common.Models.GymUser;
using Application.Common.Models.GymWorker;
using Application.Enums;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
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
        private readonly string password = "Bngym2023";

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
                return Result.Failure("Pogrešan email ili lozinka");

            if (!user.EmailConfirmed)
                return Result.Failure("Email nije potvrdjen");

            var userRole = _dbContext.UserRoles.Where(x => x.UserId == user.Id).FirstOrDefault();
            if (userRole.RoleId != Convert.ToInt32(UserRole.Admin))
                return Result.Failure("Pristup onemogućen. Nije vam dozvoljeno prijavljivanje na ovaj sajt");

            if (user.IsBlocked)
                return Result.Failure("Korisnik je blokiran");

            var role = _dbContext.Roles.Where(x => x.Id == userRole.RoleId).FirstOrDefault();

            var claims = GetClaims(user);
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
            var token = GenerateJwtForUser(user, claims);

            // Check expiration date
            var maintenanceResult = await _maintenanceService.CheckExpirationDate();
            // Clear history
            var clearResult = await _maintenanceService.ClearCheckIns();

            return Result.Successful(token);
        }

        public async Task<Result> LoginApp(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
                return Result.Failure("Pogrešan email ili lozinka");

            if (!user.EmailConfirmed)
                return Result.Failure("Email nije potvrdjen");

            var role = _dbContext.UserRoles.Where(x => x.UserId == user.Id).FirstOrDefault();
            if (role.RoleId != Convert.ToInt32(UserRole.RegularUser))
                return Result.Failure("Pristup onemogućen. Nije vam dozvoljeno prijavljivanje na aplikaciju");


            var regularUser = _dbContext.GymUsers.Where(x => x.UserId == user.Id).FirstOrDefault();
            // Check expiration date
            var maintenanceResult = await _maintenanceService.CheckExpirationDate(user.Id);
            return Result.Successful(regularUser.Id, regularUser.UserId);
        }

        public async Task<RegisterResult> Register(string email, string firstName, string lastName, string address)
        {
            if (await _userManager.FindByEmailAsync(email) != null)
                return RegisterResult.Failure("Korisnik sa navedenim email-om već postoji");

            var user = new User
            {
                Email = email.ToLower(),
                UserName = email.ToLower(),
                FirstName = firstName,
                LastName = lastName,
                Address = address,
                EmailConfirmed = true
            };

            var registerResult = await _userManager.CreateAsync(user, password);

            if (!registerResult.Succeeded)
                throw IdentityException.RegisterException(registerResult.Errors.Select(e => e.Description));

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
                return Result.Failure("Desila se greška prilikom procesiranja reset password poziva");

            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, token, password);

            if (resetPasswordResult.Succeeded)
                return Result.Successful();
            else
                return Result.Failure("Desila se greška prilikom procesiranja reset password poziva");
        }

        public async Task<Result> GenerateTokenForIdentityPurpose(string email, TokenPurpose purpose)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return Result.Failure("Desila se greška prilikom procesiranja token zahtijeva");

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

        public async Task<Result> ChangePassword(int id, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return Result.Failure("Korisnik sa proslijedjenim id ne postoji");

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!result.Succeeded)
            {
                var message = "";
                var item = result.Errors.ElementAt(0);

                if (item.Code == "PasswordMismatch")
                    message = "Pogrešno ste unijeli trenutnu loziku";
                else if (item.Code == "PasswordRequiresLower")
                    message = "Nova lozinka mora sadržati barem jedno malo slovo";

                return Result.Failure("Nije moguće promijeniti šifru. " + message);
            }


            return Result.Successful();
        }
    }
}