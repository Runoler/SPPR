using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253503_BARANCHIK.Domain.Entities
{
    public class RoomCategory
    {
        public int Id { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        public string NormalizedName { get; set; }

        public RoomCategory() { }

        public RoomCategory(int id, string name, string normalizedName)
        {
            Id = id;
            Name = name;
            NormalizedName = normalizedName;
        }
    }
}
