using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253503_BARANCHIK.Domain.Entities;
using WEB_253503_BARANCHIK.UI.Services.RoomService;
using WEB_253503_BARANCHIK.UI.Services.RoomCategoryService;

namespace WEB_253503_BARANCHIK.UI.Areas.AdminArea.Views.Admin
{
    public class CreateModel : PageModel
    {
        private readonly IRoomService _roomService;
        private readonly IRoomCategoryService _roomCategoryService;

        public CreateModel(IRoomService roomService, IRoomCategoryService roomCategoryService)
        {
            _roomService = roomService;
            _roomCategoryService = roomCategoryService;
        }

        [BindProperty]
        public Room Room { get; set; } = default!;
        [BindProperty]
        public IFormFile? Image { get; set; }
        public List<RoomCategory> Categories { get; set; } = new List<RoomCategory>();

        public async Task<IActionResult> OnGetAsync()
        {
            var response = await _roomCategoryService.GetRoomCategoryListAsync();
            if (response.Successfull)
            {
                Categories = response.Data;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unable to load categories.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            try
            {
                await _roomService.CreateRoomAsync(Room, Image);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error creating product: {ex.Message}");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
