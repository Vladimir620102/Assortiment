namespace Assortment.Web.ViewComponents
{
    public class MenuItem
    {
        public string Title { get; set; } = null!;
        public string? Controller { get; set; }
        public string? Action { get; set; } = "Index";
        public string? Icon { get; set; }
        /// <summary>
        /// true — это заголовок группы (не кликабельный).
        /// false — обычный пункт меню (ссылка).
        /// </summary>
        public bool IsGroup { get; set; }
        public List<MenuItem> Children { get; set; } = new();
    }
}
