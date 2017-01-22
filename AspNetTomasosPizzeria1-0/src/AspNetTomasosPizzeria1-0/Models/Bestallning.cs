using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspNetTomasosPizzeria1_0.Models
{
    public partial class Bestallning
    {
        public Bestallning()
        {
            BestallningMatratt = new HashSet<BestallningMatratt>();

            BestallningDatum = DateTime.Now;

            

        }


        public int BestallningId { get; set; }

        public DateTime BestallningDatum { get; set; }

        [Required]
        public int Totalbelopp { get; set; }

        [Required]
        public bool Levererad { get; set; }
        

        public virtual ICollection<BestallningMatratt> BestallningMatratt { get; set; }
        public virtual Kund Kund { get; set; }
        public int KundId { get; set; }


    }
}
