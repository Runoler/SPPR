using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253503_BARANCHIK.Domain.Entities;
using WEB_253503_BARANCHIK.UI.Services.RoomService;

namespace WEB_253503_BARANCHIK.UI.Areas.AdminArea.Views.Admin
{
    public class IndexModel : PageModel
    {
        private readonly IRoomService _roomService;

        public IndexModel(IRoomService roomService)
        {
            _roomService = roomService;
            Rooms = new List<Room>();
        }
        [BindProperty]
        public List<Room> Rooms{ get; set; }
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; }
        [BindProperty]
        public int TotalPages { get; set; }
        public async Task<IActionResult> OnGetAsync(int pageNo = 1)
        {
            var response = await _roomService.GetRoomListAsync(null, pageNo);
            if (response?.Data?.Items != null)
            {
                Rooms = response.Data.Items;
                CurrentPage = response.Data.CurrentPage;
                TotalPages = response.Data.TotalPages;
            }
            return Page();
        }
    }
}