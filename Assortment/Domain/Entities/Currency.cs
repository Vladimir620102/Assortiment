using System.ComponentModel.DataAnnotations;

namespace Assortment.Domain.Entities
{
    public class Currency
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(3, MinimumLength = 3), Display(Name = "Код ISO (3 буквы)")]
        public string IsoCode { get; set; } = null!;         // "USD", "CNY", "EUR", "BYN", "RUB"

        [Required, StringLength(100), Display(Name = "Название")]
        public string Name { get; set; } = null!;

        [StringLength(10), Display(Name = "Символ")]
        public string? Symbol { get; set; }                  // "$", "¥", "€", "Br"

        [Range(0, 6), Display(Name = "Знаков после запятой")]
        public int DecimalPlaces { get; set; } = 2;

        [Display(Name = "Активна")]
        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedAt { get; set; }

    }
}
