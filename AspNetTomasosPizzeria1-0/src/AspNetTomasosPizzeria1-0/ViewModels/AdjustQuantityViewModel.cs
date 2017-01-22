using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetTomasosPizzeria1_0.Models;

namespace AspNetTomasosPizzeria1_0.ViewModels
{
    public class AdjustQuantityViewModel
    {
        public List<CartItem> Cart { get; set; }

        public CartItem CartItem { get; set; }
    }
}
