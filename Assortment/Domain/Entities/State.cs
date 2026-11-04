using System.ComponentModel.DataAnnotations;

namespace Assortment.Domain.Entities
{
    public class State
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, Display(Name = "Страна")]
        public Guid CountryId { get; set; }
        public Country Country { get; set; } = null!;

        [Required, StringLength(100), Display(Name = "Название")]
        public string Name { get; set; } = null!;

        [StringLength(100), Display(Name = "Перевод")]
        public string? Translate { get; set; }

        [Display(Name = "Активен")]
        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedAt { get; set; }

        public ICollection<City> Cities { get; set; } = new List<City>();
    }
}
