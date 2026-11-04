using System.ComponentModel.DataAnnotations;

namespace Assortment.Domain.Entities
{
    public enum ComponentType
    {
        [Display(Name = "Материал верха")] Upper = 1,
        [Display(Name = "Подкладка")] Lining = 2,
        [Display(Name = "Стелька")] Insole = 3,
        [Display(Name = "Подошва")] Sole = 4,
        [Display(Name = "Другое")] Other = 5
    }

    public class ModelMaterial
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid ModelId { get; set; }
        public Model Model { get; set; } = null!;

        [Required]
        public Guid MaterialId { get; set; }
        public Material Material { get; set; } = null!;

        [Required, Display(Name = "Компонент")]
        public ComponentType ComponentType { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
