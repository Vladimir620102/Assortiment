using Microsoft.AspNetCore.Mvc;

namespace Assortment.Web.ViewComponents;

public class MainMenuViewComponent : ViewComponent
{
    //public IViewComponentResult Invoke()
    //{
    //    var menu = new List<MenuItem>
    //    {
    //        new()
    //        {
    //            Title = "Справочники",
    //            Icon = "bi-book",
    //            Children = new()
    //            {
    //                new() { Title = "Сезоны", Controller = "Seasons" },
    //                new() { Title = "Регионы", Controller = "Regions" },
    //                new() { Title = "Страны", Controller = "Countries" },
    //                new() { Title = "Провинции", Controller = "States" },
    //                new() { Title = "Города", Controller = "Cities" },
    //                new() { Title = "Бренды", Controller = "Brands" },
    //                new() { Title = "Коллекции", Controller = "Collections" },
    //                new() { Title = "Материалы", Controller = "Materials" },
    //            }
    //        },
    //        new()
    //        {
    //            Title = "Модели",
    //            Icon = "bi-box-seam",
    //            Children = new()
    //            {
    //                new() { Title = "Все модели", Controller = "Models" },
    //            }
    //        },
    //    };
    //    return View(menu);
    //}

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
                    new()
                    {
                        IsGroup = true,
                        Title = "Системные",
                        Children = new()
                        {
                            new() { Title = "Валюты", Controller = "Currencies" },   // ← добавлено
                            // new() { Title = "Пользователи", Controller = "Users" },
                            // new() { Title = "Роли",         Controller = "Roles" },
                        }
                    },
                    new()
                    {
                        IsGroup = true,
                        Title = "Общие",
                        Children = new()
                        {
                            new() { Title = "Регионы",   Controller = "Regions" },
                            new() { Title = "Страны",    Controller = "Countries" },
                            new() { Title = "Провинции", Controller = "States" },
                            new() { Title = "Города",    Controller = "Cities" },
                            // new() { Title = "Контрагенты", Controller = "Counterparties" },
                            // new() { Title = "Фабрики",     Controller = "Factories" },
                        }
                    },
                    new()
                    {
                        IsGroup = true,
                        Title = "Маркетинг",
                        Children = new()
                        {
                            new() { Title = "Сезоны",     Controller = "Seasons" },
                            new() { Title = "Бренды",     Controller = "Brands" },
                            new() { Title = "Коллекции",  Controller = "Collections" },
                            new() { Title = "Материалы",  Controller = "Materials" },
                        }
                    },
                    new()
                    {
                        IsGroup = true,
                        Title = "Логистика",
                        Children = new()
                        {
                            // new() { Title = "Склады",    Controller = "Warehouses" },
                            // new() { Title = "Валюты",    Controller = "Currencies" },
                            // new() { Title = "Условия поставки", Controller = "Incoterms" },
                        }
                    },
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

        RemoveEmptyGroups(menu);
        return View(menu);
    }

    /// <summary>
    /// Убирает группы без пунктов, чтобы не показывать пустые заголовки.
    /// Когда добавится первый пункт в «Системные» или «Логистику» —
    /// группа появится автоматически.
    /// </summary>
    private static void RemoveEmptyGroups(List<MenuItem> items)
    {
        foreach (var item in items)
            if (item.Children.Count > 0)
                RemoveEmptyGroups(item.Children);

        items.RemoveAll(i => i.IsGroup && i.Children.Count == 0);
    }
}