using Humanizer.Localisation.TimeToClockNotation;
using Microsoft.AspNetCore.Mvc;
using WEB_253503_BARANCHIK.Domain.Entities;
using WEB_253503_BARANCHIK.Domain.Models;
using WEB_253503_BARANCHIK.UI.Services.RoomCategoryService;

namespace WEB_253503_BARANCHIK.UI.Services.RoomService
{
    public class MemoryRoomService : IRoomService
    {
        List<Room> _rooms;
        private IConfiguration _config;
        List<RoomCategory> _categories;
        
        public MemoryRoomService([FromServices] IConfiguration config, IRoomCategoryService categoryService)
        {
            _config = config;
            _categories = categoryService.GetRoomCategoryListAsync().Result.Data;
            SetupData();
        }

        private void SetupData()
        {
            _rooms  = new List<Room>()
            {
                //new Room { Id = 1, Name = "1", Description = "Сломан кран", Cost = 30, Image = "Images/1.jpg",
                //    Category = _categories.Find(c => c.NormalizedName.Equals("standart")) },
                //new Room { Id = 2, Name = "2", Description = "Микроволновка барахлит", Cost = 45, Image = "Images/2.jpg",
                //    Category = _categories.Find(c => c.NormalizedName.Equals("medium")) },
                //new Room { Id = 3, Name = "3", Description = "Дверь скрипит", Cost = 45, Image = "Images/3.jpg",
                //    Category = _categories.Find(c => c.NormalizedName.Equals("semi-luxury")) },
                //new Room { Id = 4, Name = "4", Description = "Пол проваливается", Cost = 45, Image = "Images/4.jpg",
                //    Category = _categories.Find(c => c.NormalizedName.Equals("luxury")) },
                //new Room { Id = 5, Name = "5", Description = "Свет моргает", Cost = 45, Image = "Images/5.jpg",
                //    Category = _categories.Find(c => c.NormalizedName.Equals("premium")) }
            };
        }

        public Task<ResponseData<Room>> CreateRoomAsync(Room room, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRoomAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Room>> GetRoomByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<RoomListModel<Room>>> GetRoomListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            int pageSize = _config.GetValue<int>("ItemsPerPage");

            var filteredRooms = _rooms
                .Where(d => categoryNormalizedName == null ||
                            d.Category.NormalizedName.Equals(categoryNormalizedName))
                .ToList();

            var totalRooms = filteredRooms.Count;
            if (totalRooms == 0)
            {
                return new ResponseData<RoomListModel<Room>>
                {
                    Successfull = false,
                    ErrorMessage = "No rooms found for the specified category.",
                    Data = new RoomListModel<Room>
                    {
                        Items = new List<Room>(),
                        CurrentPage = pageNo,
                        TotalPages = 0
                    }
                };
            }

            var roomsToReturn = filteredRooms
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var response = new RoomListModel<Room>
            {
                Items = roomsToReturn,
                CurrentPage = pageNo,
                TotalPages = (int)Math.Ceiling((double)totalRooms / pageSize)
            };

            return new ResponseData<RoomListModel<Room>>
            {
                Successfull = true,
                Data = response
            };
        }

        public Task<ResponseData<Room>> UpdateRoomAsync(int id, Room room, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
