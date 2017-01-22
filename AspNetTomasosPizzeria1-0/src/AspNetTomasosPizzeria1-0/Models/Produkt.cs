using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspNetTomasosPizzeria1_0.Models
{
    public partial class Produkt
    {
        public Produkt()
        {
            MatrattProdukt = new HashSet<MatrattProdukt>();
        }

        public int ProduktId { get; set; }
        
        [Required(ErrorMessage = "Namn är obligatoriskt")]
        [StringLength(50)]
        [Display(Name="Namn")]
        public string ProduktNamn { get; set; }

        public virtual ICollection<MatrattProdukt> MatrattProdukt { get; set; }
    }
}
