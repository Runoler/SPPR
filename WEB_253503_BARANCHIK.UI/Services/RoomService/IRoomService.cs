using WEB_253503_BARANCHIK.Domain.Entities;
using WEB_253503_BARANCHIK.Domain.Models;

namespace WEB_253503_BARANCHIK.UI.Services.RoomService
{
    public interface IRoomService
    {
        public Task<ResponseData<RoomListModel<Room>>> GetRoomListAsync(string? categoryNormalizedName, int pageNo = 1);
        public Task<ResponseData<Room>> GetRoomByIdAsync(int id);
        public Task<ResponseData<Room>> UpdateRoomAsync(int id, Room room, IFormFile? formFile);
        public Task DeleteRoomAsync(int id);
        public Task<ResponseData<Room>> CreateRoomAsync(Room room, IFormFile? formFile);
    }
}
