using System.ComponentModel.DataAnnotations;

namespace Assortment.Domain.Entities
{
    public class Brand
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(100), Display(Name = "Название")]
        public string Name { get; set; } = null!;

        [StringLength(50), Display(Name = "Код")]
        public string? Code { get; set; }

        [StringLength(100), Display(Name = "Перевод")]
        public string? Translate { get; set; }

        [Display(Name = "Активен")]
        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
