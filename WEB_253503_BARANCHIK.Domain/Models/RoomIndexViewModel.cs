using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_253503_BARANCHIK.Domain.Entities;

namespace WEB_253503_BARANCHIK.Domain.Models
{
    public class RoomIndexViewModel
    {
        public IEnumerable<RoomCategory> Categories { get; set; }
        public IEnumerable<Room> Rooms { get; set; }
        public string? SelectedCategory { get; set; }
    }
}
