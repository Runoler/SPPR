using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_253503_BARANCHIK.Domain.Entities;
using WEB_253503_Baranchik.API.Data;
using WEB_253503_BARANCHIK.Domain.Models;
using WEB_253503_Baranchik.API.Services.RoomCategoryService;

namespace WEB_253503_Baranchik.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomCategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private IRoomCategoryService _roomCategoryService;

        public RoomCategoriesController(AppDbContext context, IRoomCategoryService roomCategoryService)
        {
            _context = context;
            _roomCategoryService = roomCategoryService;
        }

        // GET: api/RoomCategories
        [HttpGet]
        public async Task<ActionResult<ResponseData<List<RoomCategory>>>> GetRoomCategories()
        {
            return Ok(await _roomCategoryService.GetRoomCategoryListAsync());
        }

        // GET: api/RoomCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomCategory>> GetRoomCategory(int id)
        {
            var roomCategory = await _context.RoomCategories.FindAsync(id);

            if (roomCategory == null)
            {
                return NotFound();
            }

            return roomCategory;
        }

        // PUT: api/RoomCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoomCategory(int id, RoomCategory roomCategory)
        {
            if (id != roomCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(roomCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomCategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/RoomCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RoomCategory>> PostRoomCategory(RoomCategory roomCategory)
        {
            _context.RoomCategories.Add(roomCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoomCategory", new { id = roomCategory.Id }, roomCategory);
        }

        // DELETE: api/RoomCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoomCategory(int id)
        {
            var roomCategory = await _context.RoomCategories.FindAsync(id);
            if (roomCategory == null)
            {
                return NotFound();
            }

            _context.RoomCategories.Remove(roomCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoomCategoryExists(int id)
        {
            return _context.RoomCategories.Any(e => e.Id == id);
        }
    }
}
