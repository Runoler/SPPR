using Microsoft.AspNetCore.Mvc;
using WEB_253503_BARANCHIK.Domain.Entities;
using WEB_253503_Baranchik.API.Data;
using WEB_253503_Baranchik.API.Services.RoomService;
using WEB_253503_BARANCHIK.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Diagnostics;
using NuGet.Common;

namespace WEB_253503_Baranchik.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private IRoomService _roomService;
        private JsonSerializerOptions _serializerOptions;

        public RoomsController(AppDbContext context, IRoomService roomService, JsonSerializerOptions serializerOptions)
        {
            _context = context;
            _roomService = roomService;
            _serializerOptions = serializerOptions;
        }

        // GET: api/Rooms
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseData<RoomListModel<Room>>>> GetRooms(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            return  Ok(await _roomService.GetRoomListAsync(categoryNormalizedName, pageNo, pageSize));
        }

        // GET: api/Rooms/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            return Ok(room);
        }

        // PUT: api/Rooms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> PutRoom(int id)
        {
            var form = Request.Form;
            var product = form["room"];
            Debug.WriteLine("Received JSON: " + product);
            var room = JsonSerializer.Deserialize<Room>(form["room"], _serializerOptions);
            var files = form.Files;

            if (id != room.Id)
            {
                return BadRequest("ID не совпадает с ID номера.");
            }

            if (GetRoom(id) == null)
            {
                return NotFound("Номер не найден.");
            }

            var result = await _roomService.UpdateRoomAsync(id, room);

            return Ok(room);
        }

        // POST: api/Rooms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Room>> PostRoom(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoom", new { id = room.Id }, room);
        }

        // DELETE: api/Rooms/5
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }
    }
}
