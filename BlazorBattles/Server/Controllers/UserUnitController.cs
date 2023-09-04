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
