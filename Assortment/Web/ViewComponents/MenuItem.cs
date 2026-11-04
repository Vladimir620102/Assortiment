namespace Assortment.Web.ViewComponents
{
    public class MenuItem
    {
        public string Title { get; set; } = null!;
        public string? Controller { get; set; }
        public string? Action { get; set; } = "Index";
        public string? Icon { get; set; }
        public List<MenuItem> Children { get; set; } = new();
    }
}
