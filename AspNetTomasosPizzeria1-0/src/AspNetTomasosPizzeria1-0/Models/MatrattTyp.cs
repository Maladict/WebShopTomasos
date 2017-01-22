﻿using System;
using System.Collections.Generic;

namespace AspNetTomasosPizzeria1_0.Models
{
    public partial class MatrattTyp
    {
        public MatrattTyp()
        {
            Matratt = new HashSet<Matratt>();
        }

        public int MatrattTyp1 { get; set; }
        public string Beskrivning { get; set; }

        public virtual ICollection<Matratt> Matratt { get; set; }
    }
}
