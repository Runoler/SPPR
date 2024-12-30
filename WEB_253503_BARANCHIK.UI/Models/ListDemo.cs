using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Server.HttpSys;

namespace WEB_253503_BARANCHIK.UI.Models
{
    public class ListDemo
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ListDemoViewModel
    {
        public ListDemoViewModel()
        {
            SelectedItemId = 0;
            Items = null;
        }
        public int SelectedItemId { get; set; }
        public IEnumerable<SelectListItem> Items { get; set; }
    }
}
