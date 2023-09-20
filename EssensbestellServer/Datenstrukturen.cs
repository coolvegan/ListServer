using System;
using System.Collections.Generic;

namespace EssensbestellServer
{
	public class Essenstermin
	{
		public int ID { get; set; } // Ein eindeutiger Identifikator für den Termin
		public DateTime Datum { get; set; } // Das Datum des Essensanlasses
		public string Ort { get; set; } // Der Ort des Essens
		public List<Benutzer> Teilnehmer { get; set; } // Eine Liste der Benutzer, die am Essen teilnehmen
		public Speisekarte Speisekarte { get; set; } // Die Speisekarte für diesen Anlass
	}

	public class Speisekarte
	{
		public List<string> Gerichte { get; set; } // Eine Liste von Gerichten
	}

	public class Benutzer
	{
		public string Name { get; set; } // Der Name des Benutzers
		public string AusgewaehltesGericht { get; set; } // Das vom Benutzer ausgewählte Gericht
	}
}
