using System.ComponentModel.DataAnnotations;

namespace Assortment.Domain.Entities
{
    public enum FactorySpecialty
    {
        [Display(Name = "Обувь")] Footwear = 1,
        [Display(Name = "Одежда")] Clothing = 2,
        [Display(Name = "Универсальная")] Universal = 3
    }

    public enum FactoryOwnership
    {
        [Display(Name = "Собственная")] Own = 1,
        [Display(Name = "Партнёрская")] Partner = 2,
        [Display(Name = "Аутсорс")] Outsourced = 3
    }

    public class Factory
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(50), Display(Name = "Код")]
        public string Code { get; set; } = null!;

        [Required, StringLength(200), Display(Name = "Название")]
        public string Name { get; set; } = null!;

        [Required, Display(Name = "Город")]
        public Guid CityId { get; set; }
        public City City { get; set; } = null!;

        [Display(Name = "Специализация")]
        public FactorySpecialty Specialty { get; set; }

        [Display(Name = "Тип")]
        public FactoryOwnership Ownership { get; set; }

        [StringLength(500), Display(Name = "Адрес")]
        public string? Address { get; set; }

        [StringLength(200), Display(Name = "Контактное лицо")]
        public string? ContactPerson { get; set; }

        [StringLength(100), Display(Name = "Телефон")]
        public string? Phone { get; set; }

        [StringLength(200), EmailAddress, Display(Name = "Email")]
        public string? Email { get; set; }

        [Display(Name = "Активна")]
        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedAt { get; set; }

        public ICollection<CounterpartyFactory> Counterparties { get; set; } = new List<CounterpartyFactory>();
        public ICollection<FactoryImporterBeneficiary> ImporterBeneficiaryRules { get; set; } = new List<FactoryImporterBeneficiary>();
    }
}
