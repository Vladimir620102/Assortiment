using System.ComponentModel.DataAnnotations;

namespace Assortment.Domain.Entities
{
    public class Region
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(100), Display(Name = "Название")]
        public string Name { get; set; } = null!;

        [StringLength(100), Display(Name = "Перевод")]
        public string? Translate { get; set; }

        [StringLength(50), Display(Name = "Внешний код")]
        public string? ExternalCode { get; set; }

        [Display(Name = "Активен")]
        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedAt { get; set; }

        public ICollection<Country> Countries { get; set; } = new List<Country>();
    }
}
