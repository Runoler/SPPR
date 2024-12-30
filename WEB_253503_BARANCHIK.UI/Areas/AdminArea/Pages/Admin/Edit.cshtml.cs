using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253503_BARANCHIK.Domain.Entities;
using WEB_253503_BARANCHIK.UI.Services.RoomService;
using WEB_253503_BARANCHIK.UI.Services.RoomCategoryService;
using System.Diagnostics;

namespace WEB_253503_BARANCHIK.UI.Areas.AdminArea.Views.Admin
{
    public class EditModel : PageModel
    {
        private readonly IRoomService _roomService;
        private readonly IRoomCategoryService _roomCategoryService;

        public EditModel(IRoomService roomService, IRoomCategoryService roomCategoryService)
        {
            _roomService = roomService;
            _roomCategoryService = roomCategoryService;
        }

        [BindProperty]
        public Room Room { get; set; } = default!;
        [BindProperty]
        public IFormFile? Image { get; set; }
        [BindProperty]
        public List<RoomCategory> Categories { get; set; } = new List<RoomCategory>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var response = await _roomCategoryService.GetRoomCategoryListAsync();
            if (response.Successfull)
            {
                Categories = response.Data;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unable to load categories.");
            }
            var roomResponse = await _roomService.GetRoomByIdAsync(id);
            Debug.WriteLine("response" + roomResponse);
            if (roomResponse == null || roomResponse.Data == null)
            {
                return NotFound();
            }
            Room = roomResponse.Data;
            Room.Id = id;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _roomService.UpdateRoomAsync(Room.Id, Room, Image);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error updating product: {ex.Message}");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
