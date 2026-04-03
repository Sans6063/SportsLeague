using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;
using System.Runtime.InteropServices;


namespace SportsLeague.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SponsorController : ControllerBase
    {
        private readonly ISponsorService _sponsorService;
        private readonly IMapper _mapper;

        public SponsorController(ISponsorService sponsorService, IMapper mapper)
        {
            _sponsorService = sponsorService;
            _mapper = mapper;
        }

        [HttpGet]

        public async Task<ActionResult> GetAll()
        {
            var sponsors = await _sponsorService.GetAllAsync();
            var result = _mapper.Map<IEnumerable<SponsorResponseDTO>>(sponsors);

            return Ok(result);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult> GetById (int id)
        {
            try 
            {
                var sponsor = await _sponsorService.GetByIdAsync(id);
                var result = _mapper.Map<SponsorResponseDTO>(sponsor);

                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]

        public async Task<ActionResult> Create(SponsorRequestDTO dto)
        {
            try
            {
                var sponsor = _mapper.Map<Sponsor>(dto);
                var created = await _sponsorService.CreateAsync(sponsor);

                var result = _mapper.Map<SponsorResponseDTO>(created);

                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch(InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
        [HttpPut("{id}")]

        public async Task<ActionResult> Update(int id, SponsorRequestDTO dto)
        {
            try
            {
                var sponsor = _mapper.Map<Sponsor>(dto);
                await _sponsorService.UpdateAsync(id, sponsor);
                return NoContent();
            }
            catch(KeyNotFoundException)
            {
                return NotFound();
            }
            catch(InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _sponsorService.DeleteAsync(id);
                return NoContent();
            }
            catch(KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
