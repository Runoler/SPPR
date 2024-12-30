using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253503_BARANCHIK.UI.Models;

namespace WEB_253503_BARANCHIK.UI.Controllers
{

    public class Home : Controller
    {
        public IActionResult Index()
        {
            var items = new List<ListDemo>
            {
                new ListDemo { Id = 1, Name = "Элемент 1" },
                new ListDemo { Id = 2, Name = "Элемент 2" },
                new ListDemo { Id = 3, Name = "Элемент 3" }
            };
            ViewData["LabNum"] = "Лабораторная работа 2";
            ViewBag.ItemList = new SelectList(items, "Id", "Name");
            return View();
        }
    }
}
