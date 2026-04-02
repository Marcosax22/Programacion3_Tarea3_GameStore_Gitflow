//Marcos Ariel 2024-1785
using GameStore.API.Data;
using GameStore.API.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly GameStoreDbContext _context;
        private readonly ILogger<GamesController> _logger;

        public GamesController(GameStoreDbContext context, ILogger<GamesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("List")]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetAll()
        {
            var entities = await _context.Games
                .AsNoTracking()
                .OrderBy(g => g.Name)
                .ToListAsync();

            var dtos = entities.Select(e => e.ToDto());
            return Ok(dtos);
        }

        [HttpGet("Details/{id:int}")]
        public async Task<ActionResult<GameDto>> GetById(int id)
        {
            var entity = await _context.Games
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id);

            if (entity == null)
                return NotFound(new { message = "Game not found." });

            return Ok(entity.ToDto());
        }

        [HttpPost("Create")]
        public async Task<ActionResult<GameDto>> Create([FromBody] GameCreateDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = input.ToEntity();

            var exists = await _context.Games.AnyAsync(g => g.Name.ToLower() == entity.Name.ToLower());
            if (exists)
                return Conflict(new { message = "A game with the same name already exists." });

            _context.Games.Add(entity);
            await _context.SaveChangesAsync();

            var dto = entity.ToDto();
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] GameUpdateDto update)
        {
            if (id != update.Id)
                return BadRequest(new { message = "Id in route and body do not match." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = await _context.Games.FirstOrDefaultAsync(g => g.Id == id);
            if (entity == null)
                return NotFound(new { message = "Game not found." });

            update.MapToEntity(entity);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error updating game {GameId}", id);
                return StatusCode(500, new { message = "Unexpected concurrency error." });
            }

            return NoContent();
        }

        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Games.FirstOrDefaultAsync(g => g.Id == id);
            if (entity == null)
                return NotFound(new { message = "Game not found." });

            _context.Games.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}