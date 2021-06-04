using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Temperatura.Models
{
    public class Dados
    {
        public string CityName { get; set; }
        public List<Resultados> Resultados { get; set; }
    }

    public class Resultados
    {
        public string Date { get; set; }
        public string Weekday { get; set; }
        public int Max { get; set; }
        public int Min { get; set; }
        public string Description { get; set; }
        public string Condition { get; set; }
    }

}