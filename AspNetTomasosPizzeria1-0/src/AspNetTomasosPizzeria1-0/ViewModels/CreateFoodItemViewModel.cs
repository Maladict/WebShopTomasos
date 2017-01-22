using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AspNetTomasosPizzeria1_0.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetTomasosPizzeria1_0.ViewModels
{
    public class CreateFoodItemViewModel
    {
        [Required(ErrorMessage = "Namn är obligatoriskt")]
        [Display(Name = "Namn")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Beskrivning är obligatorisk")]
        [Display(Name = "Beskrivning")]
        [StringLength(200)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Pris är obligatoriskt")]
        [Display(Name = "Pris")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Typ är obligatorisk")]
        [Display(Name = "Typ")]
        public int Type { get; set; }

        [Required(ErrorMessage = "Ingredienser är obligatoriska")]
        [Display(Name = "Ingredienser")]
        public int[] IngredientsChosen { get; set; }

        public IEnumerable<SelectListItem> IngredientsOptions { set; get; }
        public IEnumerable<SelectListItem> TypeOptions { set; get; }

    }
}