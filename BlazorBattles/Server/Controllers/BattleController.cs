﻿using BlazorBattles.Models.Dto;
using BlazorBattles.Server.Extensions;
using DataAccess.Data.Domain;
using DataAccess.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace BlazorBattles.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BattleController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserUnitRepository _userUnitRepository;
        private readonly IBattleRepository _battleRepository;

        public BattleController(UserManager<User> userManager, IUserUnitRepository userUnitRepository, IBattleRepository battleRepository)
        {
            _userManager = userManager;
            _userUnitRepository = userUnitRepository;
            _battleRepository = battleRepository;
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

            var result = new BattleResultDTO();
            await Fight(attacker, opponent, result);

            return Ok(result);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var user = await _userManager.FindByIdAsync(User.GetUserId());
            var battles = await _battleRepository
                .GetAllFilteredAsync(battle => battle.AttackerId == user.Id || battle.OpponentId == user.Id,
                "Attacker,Opponent,Winner");

            var history = battles.Select(battle => new BattleHistoryDTO
            {
                BattleId = battle.Id,
                AttackerId = battle.AttackerId,
                OpponentId = battle.OpponentId,
                YouWon = battle.WinnerId == user.Id,
                AttackerName = battle.Attacker.UserName,
                OpponentName = battle.Opponent.UserName,
                RoundsFought = battle.RoundsFought,
                WinnerDamageDealt = battle.WinnerDamage,
                BattleDate = battle.BattleDate
            });
            return Ok(history.OrderByDescending(h => h.BattleDate));
        }

        private async Task Fight(User? attacker, User opponent, BattleResultDTO result)
        {
            if (attacker != null)
            {
                List<UserUnit> attackerArmy = (List<UserUnit>) await _userUnitRepository.GetAllFilteredAsync(u => u.UserId == attacker.Id && u.HitPoints > 0, "Unit");
                List<UserUnit> opponentArmy = (List<UserUnit>)await _userUnitRepository.GetAllFilteredAsync(u => u.UserId == opponent.Id && u.HitPoints > 0, "Unit");


                var attackerDamageSum = 0;
                var opponentDamageSum = 0;

                int currentRound = 0;
                // fight till the bitter end !
                while (attackerArmy.Count > 0 && opponentArmy.Count > 0)
                {
                    currentRound++;

                    if (currentRound % 2 != 0) // om de  beurt!
                        attackerDamageSum += FightRound(attacker, opponent, attackerArmy, opponentArmy, result);
                    else
                        opponentDamageSum += FightRound(opponent, attacker, opponentArmy, attackerArmy, result);
                }

                result.IsVictory = opponentArmy.Count == 0;
                result.RoundsFought = currentRound;

                if (result.RoundsFought > 0)
                    await FinishFight(attacker, opponent, result, attackerDamageSum, opponentDamageSum);

            }
        }

       

        private int FightRound(User attacker, User opponent, List<UserUnit> attackerArmy, List<UserUnit> opponentArmy, BattleResultDTO result)
        {
            // pick a random unit from each.
            int randomAttackerIndex = new Random().Next(attackerArmy.Count);
            int randomOpponentIndex = new Random().Next(opponentArmy.Count);

            var randomAttacker = attackerArmy[randomAttackerIndex];
            var randomOpponent = opponentArmy[randomOpponentIndex];

            var damage =
               new Random().Next(randomAttacker.Unit.Attack) - new Random().Next(randomOpponent.Unit.Defense);
            if (damage < 0) damage = 0;

            if (damage <= randomOpponent.HitPoints)
            {
                randomOpponent.HitPoints -= damage;
                result.Log.Add(
                    $"{attacker.UserName}'s {randomAttacker.Unit.Title} attacks " +
                    $"{opponent.UserName}'s {randomOpponent.Unit.Title} with {damage} damage.");
                return damage;
            }
            else
            {
                damage = randomOpponent.HitPoints;
                randomOpponent.HitPoints = 0;
                opponentArmy.Remove(randomOpponent);
                result.Log.Add(
                    $"{attacker.UserName}'s {randomAttacker.Unit.Title} kills " +
                    $"{opponent.UserName}'s {randomOpponent.Unit.Title}!");
                return damage;
            }
        }

        private async Task FinishFight(User attacker, User opponent, BattleResultDTO result, int attackerDamageSum, int opponentDamageSum)
        {
            result.AttackerDamageSum = attackerDamageSum;
            result.OpponentDamageSum = opponentDamageSum;

            attacker.Battles++;
            opponent.Battles++;

            if (result.IsVictory) // attacker has won!
            {
                attacker.Victories++;
                opponent.Defeats++;
                attacker.Bananas += opponentDamageSum;
                opponent.Bananas += attackerDamageSum * 10; // get more bananas to revive his/her army.
            }
            else
            {
                attacker.Defeats++; // attacker has lost the battle
                opponent.Victories++;
                attacker.Bananas += opponentDamageSum * 10;
                opponent.Bananas += attackerDamageSum;
            }

            await StoreBattleHistoryAsync(attacker, opponent, result);

            await _userManager.UpdateAsync(attacker);
            await _userManager.UpdateAsync(opponent);

        }

        private async Task StoreBattleHistoryAsync(User attacker, User opponent, BattleResultDTO result)
        {
            var battle = new Battle();
            battle.Attacker = attacker;
            battle.Opponent = opponent;
            battle.RoundsFought = result.RoundsFought;
            battle.WinnerDamage = result.IsVictory ? result.AttackerDamageSum : result.OpponentDamageSum;
            battle.Winner = result.IsVictory ? attacker : opponent;

            await _battleRepository.AddAsync(battle);
        }
    }
}
