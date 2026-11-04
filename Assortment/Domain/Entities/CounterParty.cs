using System.ComponentModel.DataAnnotations;

namespace Assortment.Domain.Entities
{
    public class Counterparty
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(200), Display(Name = "Название")]
        public string Name { get; set; } = null!;

        [StringLength(300), Display(Name = "Полное наименование")]
        public string? FullName { get; set; }

        [StringLength(50), Display(Name = "ИНН / рег. номер")]
        public string? Inn { get; set; }

        [StringLength(100), Display(Name = "Страна регистрации")]
        public string? RegistrationCountry { get; set; }

        [StringLength(500), Display(Name = "Юридический адрес")]
        public string? LegalAddress { get; set; }

        [StringLength(100), Display(Name = "Банковский счёт")]
        public string? BankAccount { get; set; }

        [StringLength(200), Display(Name = "Банк")]
        public string? BankName { get; set; }

        [StringLength(50), Display(Name = "SWIFT")]
        public string? Swift { get; set; }

        [StringLength(200), Display(Name = "Контактное лицо")]
        public string? ContactPerson { get; set; }

        [StringLength(100), Display(Name = "Телефон")]
        public string? Phone { get; set; }

        [StringLength(200), EmailAddress, Display(Name = "Email")]
        public string? Email { get; set; }

        [Display(Name = "Активен")]
        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedAt { get; set; }

        public ICollection<CounterpartyFactory> Factories { get; set; } = new List<CounterpartyFactory>();
        public ICollection<FactoryImporterBeneficiary> ImporterRules { get; set; } = new List<FactoryImporterBeneficiary>();
        public ICollection<FactoryImporterBeneficiary> BeneficiaryRules { get; set; } = new List<FactoryImporterBeneficiary>();
    }
}
