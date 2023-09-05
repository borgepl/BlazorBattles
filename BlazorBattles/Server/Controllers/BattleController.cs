using BlazorBattles.Server.Extensions;
using DataAccess.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorBattles.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BattleController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public BattleController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> StartBattle([FromBody] string opponentId)
        {
            var attacker = await _userManager.FindByIdAsync(User.GetUserId());
            var opponent = await _userManager.FindByIdAsync(opponentId);
            if (opponent == null || opponent.IsDeleted)
            {
                return NotFound("Opponent not available.");
            }

            return Ok();
        }
    }
}
