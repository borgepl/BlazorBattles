using BlazorBattles.Models.Dto.Auth;
using BlazorBattles.Server.Services.Contracts;
using DataAccess.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlazorBattles.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class AuthController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;

        public AuthController(SignInManager<User> signInManager,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            ITokenService tokenService)
        {
            _roleManager = roleManager;
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
            
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(RegistrationResponseDTO), StatusCodes.Status400BadRequest )]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO request)
        {
            if (request == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = new User
            {
                UserName = request.Username,
                Email = request.Email,
                Bananas = request.Bananas,
                DateOfBirth = request.DateOfBirth,
                IsConfirmed = request.IsConfirmed,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new RegistrationResponseDTO
                { Errors = errors, IsRegisterationSuccessful = false });
            }

            //var roleResult = await _userManager.AddToRoleAsync(user, SD.Role_Customer);
            //if (!roleResult.Succeeded)
            //{
            //    var errors = result.Errors.Select(e => e.Description);
            //    return BadRequest(new RegistrationResponseDTO
            //    { Errors = errors, IsRegisterationSuccessful = false });
            //}

            var userInDb = await _userManager.FindByEmailAsync(request.Email);
            if (userInDb != null)
            {
                return Ok(new RegistrationResponseDTO { Data = userInDb.Id, IsRegisterationSuccessful = true});
            }
            
            return StatusCode(201);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO request)
        {
            var result = await _signInManager.PasswordSignInAsync(request.Username,
                request.Password, false, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(request.Username);
                if (user == null)
                {
                    return Unauthorized(new AuthenticationResponseDTO
                    {
                        IsAuthSuccessful = false,
                        ErrorMessage = "Invalid Authentication"
                    });
                }

                //everything is valid and we need to login the user
                return Ok(new AuthenticationResponseDTO
                {
                    IsAuthSuccessful = true,
                    Token = _tokenService.CreateToken(user),
                    
                    //UserDTO = new UserDTO
                    //{
                    //    Name = user.Name,
                    //    Id = user.Id,
                    //    Email = user.Email,
                    //    PhoneNo = user.PhoneNumber
                    //}
                });
            }
            else
            {
                return Unauthorized(new AuthenticationResponseDTO
                {
                    IsAuthSuccessful = false,
                    ErrorMessage = "Invalid Authentication"
                });
            }
        }
    }
}
