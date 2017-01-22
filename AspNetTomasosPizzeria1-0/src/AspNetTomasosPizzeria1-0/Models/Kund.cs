using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspNetTomasosPizzeria1_0.Models
{
    public partial class Kund
    {
        public Kund()
        {
            Bestallning = new HashSet<Bestallning>();
        }

        public int KundId { get; set; }
        [Required(ErrorMessage = "Namn är obligatoriskt.")]
        [StringLength(100)]
        public string Namn { get; set; }

        [Required(ErrorMessage = "Gatuadress är obligatorisk.")]
        [StringLength(50)]
        public string Gatuadress { get; set; }

        [Required(ErrorMessage = "Postnummer är obligatoriskt")]
        [DataType(DataType.PostalCode, ErrorMessage = "Felaktigt format.")]
        [Display(Name="Postnummer")]
        [StringLength(20)]
        public string Postnr { get; set; }

        [Required(ErrorMessage = "Postort är obligatorisk.")]
        [StringLength(100)]
        public string Postort { get; set; }

        [Required(ErrorMessage = "Epostadress är obligatorisk.")]
        [EmailAddress]
        [DataType(DataType.EmailAddress, ErrorMessage = "Felaktigt format.")]
        [Display(Name = "Epost")]
        [StringLength(50)]
        public string Email { get; set; }

        [Display(Name = "Telefonnummer")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Telefonnumret har ett felaktigt format.")]
        [StringLength(50)]
        public string Telefon { get; set; }

        [Required(ErrorMessage = "Användarnamn är obligatoriskt.")]
        [Display(Name = "Användarnamn")]
        [StringLength(20)]
        public string AnvandarNamn { get; set; }

        [Required(ErrorMessage = "Lösenord är obligatoriskt.")]
        [Display(Name = "Lösenord")]
        [DataType(DataType.Password, ErrorMessage = "Lösenordet har ett felaktigt format.")]
        [StringLength(20)]
        [MinLength(7, ErrorMessage = "Lösenordet måste vara längre än 7 tecken.")]
        public string Losenord { get; set; }

        public string Role { get; set; }

        public int Points { get; set; }

        public virtual ICollection<Bestallning> Bestallning { get; set; }
    }
}
