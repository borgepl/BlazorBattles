using BlazorBattles.Models.Dto;
using BlazorBattles.Server.Extensions;
using DataAccess.Data;
using DataAccess.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlazorBattles.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;

        public UserController(UserManager<User> userManager, DataContext context)
        {
            _userManager = userManager;
            _context = context;
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

        [HttpGet("leaderboard")]
        public async Task<IActionResult> GetLeaderboard()
        {
            var users = await _context.AppUsers.Where(user => !user.IsDeleted && user.IsConfirmed).ToListAsync();

            users = users
                .OrderByDescending(u => u.Victories)
                .ThenBy(u => u.Defeats)
                .ThenBy(u => u.DateCreated)
                .ToList();

            int rank = 1;
            var response = users.Select(user => new UserStatisticDTO
            {
                Rank = rank++,
                UserId = user.Id,
                Username = user.UserName,
                Battles = user.Battles,
                Victories = user.Victories,
                Defeats = user.Defeats
            });

            return Ok(response);
        }
    }

}
