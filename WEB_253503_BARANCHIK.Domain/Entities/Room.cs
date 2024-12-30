using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253503_BARANCHIK.Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        [Display(Name="Номер")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description {  get; set; }
        [Display(Name = "Категория")]
        public RoomCategory Category { get; set; }
        [Display(Name = "Стоимость")]
        public int Cost { get; set; }
        [Display(Name = "Фото")]
        public string Image {  get; set; }

        public Room() { }

        public Room(int id, string name, string description, RoomCategory category, int cost, string image)
        {
            Id = id;
            Name = name;
            Description = description;
            Category = category;
            Cost = cost;
            Image = image;
        }
    }
}
