using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AspNetTomasosPizzeria1_0.Models;

namespace AspNetTomasosPizzeria1_0.ViewModels
{
    public class ConfirmOrderViewModel
    {
        public Bestallning Order { get; set; }

        [Display(Name="Poängrabatt")]
        public int DiscountPoints { get; set; }
        [Display(Name="Mängdrabatt")]
        public int DiscountMultipleOrders { get; set; }

        [Display(Name="Originalpris")]
        public double OriginalPrice { get; set; }
    }
}
