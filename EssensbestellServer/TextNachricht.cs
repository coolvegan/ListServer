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
            String result = Verstecker.getInstance().gibPublic();
            result = Convert.ToBase64String(Encoding.ASCII.GetBytes(result));
            dtoNachricht.aktion = "rsa";
            dtoNachricht.data = result;
            string json = JsonSerializer.Serialize(dtoNachricht);
            TcpClient client = nachricht.Item2;
            byte[] data = Encoding.ASCII.GetBytes(json);
            var stream = client.GetStream();
            stream.Write(data, 0, data.Length);
        }
	}

	public class JsonNachricht : TextNachricht
	{
        public void ausgabe(Tuple<String, TcpClient> nachricht)
        {
			try
			{
                JsonDocument json = JsonDocument.Parse(nachricht.Item1);
                JsonElement wurzel = json.RootElement;
                Console.WriteLine(nachricht);
            }
			catch(Exception e)
			{
				base.ausgabe(nachricht);
			}

        }
    }

    public class VerschlusselteNachricht : JsonNachricht
    {
        public void ausgabe(Tuple<String, TcpClient> nachricht)
        {
            try
            {
                byte[] b =  Convert.FromBase64String(nachricht.Item1);
                String msg = Verstecker.getInstance().entschluessel(b);            
                Console.WriteLine(msg);
            }
            catch (Exception e)
            {
                base.ausgabe(nachricht);
            }

        }
    }
}

