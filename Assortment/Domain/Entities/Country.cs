using System.ComponentModel.DataAnnotations;

namespace Assortment.Domain.Entities
{
    public class Country
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, Display(Name = "Регион")]
        public Guid RegionId { get; set; }
        public Region Region { get; set; } = null!;

        [Required, StringLength(100), Display(Name = "Название")]
        public string Name { get; set; } = null!;

        [Required, StringLength(2, MinimumLength = 2), Display(Name = "ISO (2 буквы)")]
        public string IsoCode { get; set; } = null!;

        [StringLength(100), Display(Name = "Перевод")]
        public string? Translate { get; set; }

        [Display(Name = "Активна")]
        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedAt { get; set; }

        public ICollection<State> States { get; set; } = new List<State>();
    }
}
