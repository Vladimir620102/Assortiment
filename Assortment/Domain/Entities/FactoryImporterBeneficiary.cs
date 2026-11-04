using System.ComponentModel.DataAnnotations;

namespace Assortment.Domain.Entities
{
    public class FactoryImporterBeneficiary
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, Display(Name = "Фабрика")]
        public Guid FactoryId { get; set; }
        public Factory Factory { get; set; } = null!;

        [Required, Display(Name = "Импортёр")]
        public Guid ImporterId { get; set; }
        public Counterparty Importer { get; set; } = null!;

        [Required, Display(Name = "Бенефициар")]
        public Guid BeneficiaryId { get; set; }
        public Counterparty Beneficiary { get; set; } = null!;

        [Display(Name = "По умолчанию")]
        public bool IsDefault { get; set; } = true;

        [Display(Name = "Активно")]
        public bool IsActive { get; set; } = true;

        [StringLength(500), Display(Name = "Примечание")]
        public string? Comment { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}

