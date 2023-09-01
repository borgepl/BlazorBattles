using BlazorBattles.Server.Extensions;
using DataAccess.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazorBattles.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        private async Task<User> GetUser()
        {
            return await _userManager.FindByIdAsync(User.GetUserId());
        }

        [HttpGet("getbananas")]
        public async Task<IActionResult> GetBananas()
        {
            // var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // var userId = User.GetUserId();
            // var user = await _userManager.FindByIdAsync(userId);

            var user = await GetUser();

            return Ok(user.Bananas);
        }

        [HttpPut("addbananas")]
        public async Task<IActionResult> AddBananas([FromBody] int bananas)
        {
            var user = await GetUser();
            user.Bananas += bananas;

            await _userManager.UpdateAsync(user);
            return Ok(user.Bananas);
        }
    }

}
