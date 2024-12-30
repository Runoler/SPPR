using WEB_253503_BARANCHIK.Domain.Entities;
using WEB_253503_BARANCHIK.Domain.Models;

namespace WEB_253503_BARANCHIK.UI.Services.RoomCategoryService
{
    public class MemoryRoomCategoryService : IRoomCategoryService
    {
        public Task<ResponseData<List<RoomCategory>>> GetRoomCategoryListAsync()
        {
            var categories = new List<RoomCategory>
            {
            };
            var result = ResponseData<List<RoomCategory>>.Success(categories);
            return Task.FromResult(result);
        }
    }
}
