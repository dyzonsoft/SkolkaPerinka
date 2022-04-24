using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SkolkaPerinka.Shared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SkolkaPerinka.Server.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public UserController(SignInManager<User> signInManager, UserManager<User> userManager, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("register/{role}")]
        public async Task<IActionResult> Register([FromBody] UserToRegister user, string role)
        {
            string username = user.Email;
            string password = user.Password;

            User identityUser = new User
            {
                Email = username,
                UserName = username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Email,
                Phone = user.Phone,
                PasswordHash = password,

            };

            IdentityResult userIdentityResult = await _userManager.CreateAsync(identityUser, password);
            IdentityResult roleIdentityResult = await _userManager.AddToRoleAsync(identityUser, role);

            if (userIdentityResult.Succeeded == true && roleIdentityResult.Succeeded == true)
            {
                return Ok(new { userIdentityResult.Succeeded });
            }
            else
            {
                List<string> errorsToReturn = new();

                foreach (var error in userIdentityResult.Errors)
                {
                    errorsToReturn.Add(error.Description);
                }

                return BadRequest(errorsToReturn);
            }
        }

        // tvoje doména.com/api/user/signin
        [Route("signin")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] UserToSigniIn user)
        {
            string username = user.Email;
            string password = user.Password;

            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(username, password, false, false);
            if (signInResult.Succeeded == true)
            {
                User identityUser = await _userManager.FindByNameAsync(username);
                string JSONWebTokenAsString = await GenerateJSONWebToken(identityUser);
                return Ok(JSONWebTokenAsString);
            }
            else
            {
                return Unauthorized(user);
            }
        }

        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<string> GenerateJSONWebToken(User identityUser)
        {
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            SigningCredentials credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            // Claim = who is the person traying to sign in claming to be?
            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, identityUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, identityUser.Id)
            };

            IList<string> roleNames = await _userManager.GetRolesAsync(identityUser);
            claims.AddRange(roleNames.Select(roleName => new Claim(ClaimsIdentity.DefaultRoleClaimType, roleName)));

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken
            (
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                null,
                expires: DateTime.UtcNow.AddDays(365),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}
