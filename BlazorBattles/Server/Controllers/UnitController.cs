using DataAccess.Data.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Repositories.Contracts;

namespace BlazorBattles.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly IUnitRepository _unitRepository;

        public UnitController(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
        }

        //public IList<Unit> Units => new List<Unit>
        //{
        //    new Unit { Id = 1, Title = "Knight", Attack = 10, Defense = 10, BananaCost = 100},
        //    new Unit { Id = 2, Title = "Archer", Attack = 15, Defense = 5, BananaCost = 150},
        //    new Unit { Id = 3, Title = "Mage", Attack = 20, Defense = 1, BananaCost = 200}
        //};

        [HttpGet]
        public async Task<IActionResult> GetUnits()
        {
            var units = await _unitRepository.GetAllAsync();
            return Ok(units);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Unit>>> AddUnit(Unit unit)
        {
            await _unitRepository.AddAsync(unit);
            return Ok( await _unitRepository.GetAllAsync() );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUnit(int id, Unit unit)
        {
            if (await _unitRepository.Exists(id))
            {
                Unit dbUnit = await _unitRepository.GetAsync(id);

                dbUnit.Title = unit.Title;
                dbUnit.Defense = unit.Defense;
                dbUnit.Attack = unit.Attack;
                dbUnit.BananaCost = unit.BananaCost;
                dbUnit.HitPoints = unit.HitPoints;

                await _unitRepository.UpdateAsync(dbUnit);
                return Ok(dbUnit);
            }
            else
            {
                return NotFound("Unit with the given Id does not exist");
            }
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnit(int id)
        {
            if (await _unitRepository.Exists(id))
            {
                await _unitRepository.DeleteAsync(id);
                return Ok( await _unitRepository.GetAllAsync() );
            }
            else
            {
                return NotFound("Unit with the given Id does not exist");
            }

        }

    }
}
