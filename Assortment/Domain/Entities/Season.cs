using System.ComponentModel.DataAnnotations;

namespace Assortment.Domain.Entities;

public enum SeasonKind
{
    [Display(Name = "Демисезон")] Demi = 1,
    [Display(Name = "Весна-Лето")] SpringSummer = 2,
    [Display(Name = "Осень-Зима")] AutumnWinter = 3,
    [Display(Name = "Круглосезонная")] AllSeasons = 4
}

public class Season
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, Display(Name = "Тип сезона")]
    public SeasonKind Kind { get; set; }

    [Required, StringLength(100), Display(Name = "Название")]
    public string Name { get; set; } = null!;

    [StringLength(100), Display(Name = "Перевод")]
    public string? Translate { get; set; }

    [StringLength(50), Display(Name = "Внешний код")]
    public string? ExternalCode { get; set; }

    [StringLength(100), Display(Name = "Название для этикетки")]
    public string? LabelName { get; set; }

    [DataType(DataType.Date), Display(Name = "Дата истечения")]
    public DateOnly? ExpireDate { get; set; }

    [Display(Name = "Активен")]
    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }
}