using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253503_BARANCHIK.Domain.Entities;
using WEB_253503_BARANCHIK.UI.Services.RoomService;

namespace WEB_253503_BARANCHIK.UI.Areas.AdminArea.Views.Admin
{
    public class DetailsModel : PageModel
    {
        private readonly IRoomService _roomService;

        public DetailsModel(IRoomService roomService)
        {
            _roomService = roomService;
        }

        public Room Room { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var roomResponse = await _roomService.GetRoomByIdAsync(id);
            if (roomResponse == null || roomResponse.Data == null)
            {
                return NotFound();
            }

            Room = roomResponse.Data;
            return Page();
        }
    }
}
