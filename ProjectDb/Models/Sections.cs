using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDb.Models
{
    /// <summary>
    /// Object of Sections
    /// </summary>
    public class Sections
    {
        public int id { get; set; }
        public string Destination1 { get; set; }
        public string Destination2 { get; set; }
        public decimal Part { get; set; }
        public int Lenght { get; set; }
        public int LevelOfDifficulty { get; set; }
        public string GPXLink { get; set; }
        public int PartOf { get; set; }

    }
}
