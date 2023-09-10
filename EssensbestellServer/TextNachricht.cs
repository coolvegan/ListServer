using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace EssensbestellServer
{
    public class DtoNachricht
    {
        public string data { get; set; }
        public string aktion { get; set; }
    }

	public class TextNachricht
	{
		public TextNachricht()
		{
		}
		public void ausgabe(Tuple<String, TcpClient> nachricht)
		{
            DtoNachricht dtoNachricht = new DtoNachricht();
            Console.WriteLine("TextNachricht Ausgabe");
 
        }
	}

	public class JsonNachricht : TextNachricht
	{
        public void ausgabe(Tuple<String, TcpClient> nachricht)
        {
			try
			{
                Console.WriteLine("JsonNachricht Ausgabe");
                JsonDocument json = JsonDocument.Parse(nachricht.Item1);
                JsonElement wurzel = json.RootElement;
                JsonElement aktionsKnoten = wurzel.GetProperty("aktion");
                String aktion = aktionsKnoten.GetString()!;
                Console.WriteLine(aktion);
            }
			catch(Exception e)
			{
  
                base.ausgabe(nachricht);
			}

        }
    }

}

