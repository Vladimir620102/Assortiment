using System.ComponentModel.DataAnnotations;

namespace Assortment.Models.Dictionary
{
    public enum SeasonKind
    {
        [Display(Name = "Демисезон")]
        Demi = 1,

        [Display(Name = "Весна-Лето")]
        SpringSummer = 2,

        [Display(Name = "Осень-Зима")]
        AutumnWinter = 3,

        [Display(Name = "Круглосезонная")]
        AllSeasons = 4
    }
}
