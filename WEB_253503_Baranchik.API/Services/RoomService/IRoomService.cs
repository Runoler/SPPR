using WEB_253503_BARANCHIK.Domain.Entities;
using WEB_253503_BARANCHIK.Domain.Models;

namespace WEB_253503_Baranchik.API.Services.RoomService
{
    public interface IRoomService
    {
        public Task<ResponseData<RoomListModel<Room>>> GetRoomListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3);
        public Task<ResponseData<Room>> GetRoomByIdAsync(int id);
        public Task<ResponseData<bool>> UpdateRoomAsync(int id, Room room);
        public Task<ResponseData<bool>> DeleteRoomAsync(int id);
        public Task<ResponseData<Room>> CreateRoomAsync(Room room, IFormFile? formFile);
    }
}
