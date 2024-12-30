using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using WEB_253503_Baranchik.API.Data;
using WEB_253503_Baranchik.API.Services.RoomCategoryService;
using WEB_253503_BARANCHIK.Domain.Entities;
using WEB_253503_BARANCHIK.Domain.Models;

namespace WEB_253503_Baranchik.API.Services.RoomService
{
    public class RoomService : IRoomService
    {
        private readonly AppDbContext _context;
        private readonly int _maxPageSize = 20;
        public RoomService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseData<Room>> CreateRoomAsync(Room room, IFormFile? formFile)
        {
            if (room == null)
            {
                return ResponseData<Room>.Error("Product cannot be null.");
            }

            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();

            return ResponseData<Room>.Success(room);
        }

        public async Task<ResponseData<bool>> DeleteRoomAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
                return ResponseData<bool>.Success(true);
            }
            else
            {
                return ResponseData<bool>.Error("Product not found.");
            }
        }

        public async Task<ResponseData<Room>> GetRoomByIdAsync(int id)
        {
            var response = new ResponseData<Room>();
            var product = await _context.Rooms.FindAsync(id);
            return ResponseData<Room>.Success(product);
        }

        public async Task<ResponseData<RoomListModel<Room>>> GetRoomListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize) pageSize = _maxPageSize;
            var dataList = new RoomListModel<Room>();
            var query = _context.Rooms.AsQueryable();

            if (!string.IsNullOrEmpty(categoryNormalizedName))
            {
                query = query.Where(t => t.Category.NormalizedName == categoryNormalizedName);
            }

            var totalCount = await query.CountAsync();
            dataList.Items = query.OrderBy(d => d.Id)
                                    .Skip((pageNo - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();
            dataList.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            if (pageNo > dataList.TotalPages)
                return ResponseData<RoomListModel<Room>>.Error("No such page");
            dataList.CurrentPage = pageNo;
            return ResponseData<RoomListModel<Room>>.Success(dataList);
        }


        public async Task<ResponseData<bool>> UpdateRoomAsync(int id, Room room)
        {
            if (room == null || id != room.Id)
            {
                return ResponseData<bool>.Success(false);
            }

            var existingProduct = await _context.Rooms.FindAsync(id);
            if (existingProduct == null)
            {
                return ResponseData<bool>.Success(false);
            }

            existingProduct.Name = room.Name;
            existingProduct.Description = room.Description;
            existingProduct.Category.Id = room.Category.Id;
            existingProduct.Cost = room.Cost;
            existingProduct.Image = room.Image ?? existingProduct.Image;

            _context.Rooms.Update(existingProduct);
            await _context.SaveChangesAsync();
            return ResponseData<bool>.Success(true);
        }
    }
}
