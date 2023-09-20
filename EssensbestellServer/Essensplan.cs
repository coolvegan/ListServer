using System;
using System.Collections.Generic;

namespace EssensbestellServer
{
    public class Essenstermin
    {
        public DateTime Datum { get; set; }
        public string Ort { get; set; }
        public string Speisekarte { get; set; }
        public List<Bestellung> Bestellungen { get; set; } = new List<Bestellung>();
    }

    public class Bestellung
    {
        public string Benutzer { get; set; }
        public string Essen { get; set; }
    }
}
