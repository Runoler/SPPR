using WEB_253503_BARANCHIK.Domain.Entities;
using WEB_253503_BARANCHIK.Domain.Models;

namespace WEB_253503_BARANCHIK.BlazorWasm.Services.DataService
{
    public interface IDataService
    {
        event Action DataLoaded;
        List<RoomCategory> Categories { get; set; }
        List<Room> Rooms { get; set; }
        bool Success { get; set; }
        string ErrorMessage { get; set; }
        int TotalPages { get; set; }
        int CurrentPage { get; set; }
        RoomCategory SelectedCategory { get; set; }

        public Task GetRoomListAsync(int pageNo = 1);

        public Task GetRoomCategoryListAsync();
    }
}