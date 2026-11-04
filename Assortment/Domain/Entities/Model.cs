using System.ComponentModel.DataAnnotations;

namespace Assortment.Domain.Entities;

public enum Gender
{
    [Display(Name = "Мужской")] Male = 1,
    [Display(Name = "Женский")] Female = 2,
    [Display(Name = "Унисекс")] Unisex = 3,
    [Display(Name = "Детский")] Kids = 4
}

public class Model
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, StringLength(50), Display(Name = "Артикул")]
    public string Code { get; set; } = null!;

    [Required, StringLength(200), Display(Name = "Название")]
    public string Name { get; set; } = null!;

    [Required, Display(Name = "Коллекция")]
    public Guid CollectionId { get; set; }
    public Collection Collection { get; set; } = null!;

    [Display(Name = "Пол")]
    public Gender? Gender { get; set; }

    [Display(Name = "Активна")]
    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }

    public ICollection<ModelMaterial> Materials { get; set; } = new List<ModelMaterial>();
}
