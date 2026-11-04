using Microsoft.AspNetCore.Mvc;

namespace Assortment.Web.ViewComponents;

public class MainMenuViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var menu = new List<MenuItem>
        {
            new()
            {
                Title = "Справочники",
                Icon = "bi-book",
                Children = new()
                {
                    new() { Title = "Сезоны", Controller = "Seasons" },
                    new() { Title = "Регионы", Controller = "Regions" },
                    new() { Title = "Страны", Controller = "Countries" },
                    new() { Title = "Провинции", Controller = "States" },
                    new() { Title = "Города", Controller = "Cities" },
                    new() { Title = "Бренды", Controller = "Brands" },
                    new() { Title = "Коллекции", Controller = "Collections" },
                    new() { Title = "Материалы", Controller = "Materials" },
                }
            },
            new()
            {
                Title = "Модели",
                Icon = "bi-box-seam",
                Children = new()
                {
                    new() { Title = "Все модели", Controller = "Models" },
                }
            },
        };
        return View(menu);
    }
}