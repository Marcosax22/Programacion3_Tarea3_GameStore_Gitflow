//Marcos Ariel 2024-1785
using GameStore.API.Data;
using GameStore.API.Models.Dtos;
using GameStore.API.Models.Entities;
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
        public async Task<ActionResult<object>> GetAll([FromQuery] GameQueryDto query)
        {
            IQueryable<Games> gamesQuery = _context.Games.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.Trim().ToLower();
                gamesQuery = gamesQuery.Where(g =>
                    g.Name.ToLower().Contains(search) ||
                    g.Description.ToLower().Contains(search));
            }

            if (query.MinPrice.HasValue)
                gamesQuery = gamesQuery.Where(g => g.Price >= query.MinPrice.Value);

            if (query.MaxPrice.HasValue)
                gamesQuery = gamesQuery.Where(g => g.Price <= query.MaxPrice.Value);

            gamesQuery = ApplySorting(gamesQuery, query.SortBy, query.SortDirection);

            var page = query.Page < 1 ? 1 : query.Page;
            var pageSize = query.PageSize < 1 ? 10 : Math.Min(query.PageSize, 100);

            var totalRecords = await gamesQuery.CountAsync();

            var items = await gamesQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(g => g.ToDto())
                .ToListAsync();

            return Ok(new
            {
                totalRecords,
                page,
                pageSize,
                items
            });
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

        private static IQueryable<Games> ApplySorting(IQueryable<Games> query, string? sortBy, string? sortDirection)
        {
            var sortByNormalized = sortBy?.Trim().ToLower() ?? "name";
            var sortDirectionNormalized = sortDirection?.Trim().ToLower() ?? "asc";

            return (sortByNormalized, sortDirectionNormalized) switch
            {
                ("price", "desc") => query.OrderByDescending(g => g.Price).ThenBy(g => g.Name),
                ("price", "asc") => query.OrderBy(g => g.Price).ThenBy(g => g.Name),
                ("name", "desc") => query.OrderByDescending(g => g.Name),
                _ => query.OrderBy(g => g.Name)
            };
        }
    }
}