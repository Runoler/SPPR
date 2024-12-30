using WEB_253503_BARANCHIK.Domain.Entities;
using WEB_253503_BARANCHIK.Domain.Models;

namespace WEB_253503_Baranchik.API.Services.RoomCategoryService
{
    public interface IRoomCategoryService
    {
        public Task<ResponseData<List<RoomCategory>>> GetRoomCategoryListAsync();
    }
}
