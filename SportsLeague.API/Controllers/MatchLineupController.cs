using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers
{
    [ApiController]
    [Route("api/match/{matchId}/lineup")]
    public class MatchLineupController : ControllerBase
    {
        private readonly IMatchLineupService _lineupService;
        private readonly IMapper _mapper;

        public MatchLineupController(
            IMatchLineupService lineupService,
            IMapper mapper)
        {
            _lineupService = lineupService;
            _mapper = mapper;
        }

        
        [HttpPost]
        public async Task<ActionResult<MatchLineupResponseDTO>> AddPlayer(
            int matchId,
            [FromBody] CreateMatchLineupDTO dto)
        {
            try
            {
                var lineup = _mapper.Map<MatchLineup>(dto);

                var created = await _lineupService
                    .AddPlayerToLineupAsync(matchId, lineup);

                var response = _mapper
                    .Map<MatchLineupResponseDTO>(created);

                return StatusCode(201, response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatchLineupResponseDTO>>> GetLineup(
            int matchId)
        {
            var lineup = await _lineupService
                .GetMatchLineupAsync(matchId);

            var response = _mapper
                .Map<IEnumerable<MatchLineupResponseDTO>>(lineup);

            return Ok(response);
        }

        
        [HttpGet("team/{teamId}")]
        public async Task<ActionResult<IEnumerable<MatchLineupResponseDTO>>> GetTeamLineup(
            int matchId,
            int teamId)
        {
            var lineup = await _lineupService
                .GetTeamLineupAsync(matchId, teamId);

            var response = _mapper
                .Map<IEnumerable<MatchLineupResponseDTO>>(lineup);

            return Ok(response);
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(
            int matchId,
            int id)
        {
            try
            {
                await _lineupService
                    .DeletePlayerFromLineupAsync(matchId, id);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
        }
    }
}