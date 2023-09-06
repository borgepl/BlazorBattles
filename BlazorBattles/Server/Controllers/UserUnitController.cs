using AutoMapper;
using BlazorBattles.Models.Dto;
using BlazorBattles.Server.Extensions;
using DataAccess.Data.Domain;
using DataAccess.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;

namespace BlazorBattles.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserUnitController : ControllerBase
    {
        private readonly IUserUnitRepository _userUnitRepository;
        private readonly IUnitRepository _unitRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserUnitController(IUserUnitRepository userUnitRepository, IUnitRepository unitRepository, 
            UserManager<User> userManager, IMapper mapper)
        {
            _userUnitRepository = userUnitRepository;
            _unitRepository = unitRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost("revive")]
        public async Task<IActionResult> ReviveArmy()
        {
            // Give again Hitpoints randomly to the Units of the User - Cost 1000 bananas

            var user = await _userManager.FindByIdAsync(User.GetUserId());
            
            if (user == null)
            {
                return NotFound("User not found!");
            }
            else
            {
                var userUnits = _userUnitRepository.GetAllFilteredAsync(u => u.UserId == user.Id,"Unit");

                int bananaCost = 1000;

                if (user.Bananas < bananaCost)
                {
                    return BadRequest($"Not enough bananas! You need {bananaCost} bananas to revive your army.");
                }

                bool armyAlreadyAlive = true;
                foreach (var userUnit in userUnits)
                {
                    if (unit.HitPoints <= 0)
                    {
                        armyAlreadyAlive = false;
                        userUnit.HitPoints = new Random().Next(0, userUnit.Unit.HitPoints);
                        await _userUnitRepository.UpdateAsync(userUnit);
                    }
                }

                if (armyAlreadyAlive)
                {
                    return Ok("Your army is already alive.");
                }

                user.Bananas -= bananaCost;
                await _userManager.UpdateAsync(user);
                

                return Ok("Army revived!");

            }
        }

        [HttpPost]
        public async Task<IActionResult> BuildUserUnit([FromBody] int unitId)
        {
            var unit = await _unitRepository.GetAsync(unitId);
            var user = await _userManager.FindByIdAsync(User.GetUserId());

            if (user.Bananas < unit.BananaCost)
            {
                return BadRequest("Not enough bananas!");
            }

            user.Bananas -= unit.BananaCost;

            var newUserUnit = new UserUnit
            {
                UnitId = unitId,
                UserId = user.Id,
                HitPoints = unit.HitPoints,
            };

            await _userUnitRepository.AddAsync(newUserUnit);

            return Ok(newUserUnit);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserUnits()
        {
            var user = await _userManager.FindByIdAsync(User.GetUserId());
            var userUnits = await _userUnitRepository.GetAllFilteredAsync(u => u.UserId == user.Id);

            var userUnitsToReturn = _mapper.Map<IEnumerable<UserUnitResponseDTO>>(userUnits);
            return Ok(userUnitsToReturn);
        }
    }
}
