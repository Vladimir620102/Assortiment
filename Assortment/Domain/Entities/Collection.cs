using System.ComponentModel.DataAnnotations;

namespace Assortment.Domain.Entities
{
    public class Collection
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, Display(Name = "Бренд")]
        public Guid BrandId { get; set; }
        public Brand Brand { get; set; } = null!;

        [Required, Display(Name = "Сезон")]
        public Guid SeasonId { get; set; }
        public Season Season { get; set; } = null!;

        [Required, StringLength(100), Display(Name = "Название")]
        public string Name { get; set; } = null!;

        [StringLength(50), Display(Name = "Код")]
        public string? Code { get; set; }

        [Display(Name = "Активна")]
        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
