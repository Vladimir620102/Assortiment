using System.ComponentModel.DataAnnotations;

namespace Assortment.Domain.Entities
{
    public enum CounterpartyRole
    {
        [Display(Name = "Импортёр")] Importer = 1,
        [Display(Name = "Производитель")] Manufacturer = 2,
        [Display(Name = "Бенефициар")] Beneficiary = 3
        // новые роли — добавляются здесь, без миграции структуры
    }

    public class CounterpartyFactory
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid CounterpartyId { get; set; }
        public Counterparty Counterparty { get; set; } = null!;

        [Required]
        public Guid FactoryId { get; set; }
        public Factory Factory { get; set; } = null!;

        [Required, Display(Name = "Роль")]
        public CounterpartyRole Role { get; set; }

        [Display(Name = "Активна")]
        public bool IsActive { get; set; } = true;

        [StringLength(500), Display(Name = "Примечание")]
        public string? Comment { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
