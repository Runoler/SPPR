using Microsoft.EntityFrameworkCore;
using WEB_253503_Baranchik.API.Data;
using WEB_253503_BARANCHIK.Domain.Entities;
using WEB_253503_BARANCHIK.Domain.Models;

namespace WEB_253503_Baranchik.API.Services.RoomCategoryService
{
    public class RoomCategoryService : IRoomCategoryService
    {
        private readonly AppDbContext _context;
        public RoomCategoryService(AppDbContext context)
        {
            _context = context;
        }
        public Task<ResponseData<List<RoomCategory>>> GetRoomCategoryListAsync()
        {

            var categories = _context.RoomCategories;
            var datalist = new List<RoomCategory>(categories);

            var result = ResponseData<List<RoomCategory>>.Success(datalist);
            return Task.FromResult(result);
        }
    }
}
