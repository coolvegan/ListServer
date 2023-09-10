using EssensbestellServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Security.Cryptography;

using static System.Net.Mime.MediaTypeNames;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;

namespace client
{
    class TestClient
    {
        public void start() {
            try
            {
                // IP-Adresse und Port des Servers, zu dem eine Verbindung hergestellt werden soll
                string serverIP = "127.0.0.1";
                int serverPort = 12345;

                // Erstelle einen TCP-Client
                TcpClient client = new TcpClient();

                // Verbinde mit dem Server
                client.Connect(serverIP, serverPort);
                Console.WriteLine("Verbindung zum Server hergestellt.");

                // Nachricht, die an den Server gesendet werden soll
                string message = "{\"aktion\":\"y\"}";

                // Konvertiere die Nachricht in ein Byte-Array
                byte[] data = Encoding.UTF8.GetBytes(message);

                // Erhalte den NetworkStream des Clients
                NetworkStream stream = client.GetStream();

                // Sende die Nachricht an den Server
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Nachricht gesendet: " + message);

                // Empfange eine Antwort vom Server
                byte[] responseData = new byte[1024];
                int bytesRead = stream.Read(responseData, 0, responseData.Length);
                string responseMessage = Encoding.UTF8.GetString(responseData, 0, bytesRead);

                
                var obj = JsonSerializer.Deserialize<EssensbestellServer.DtoNachricht>(responseMessage);
                if(obj == null) {
                    client.Close();
                    return;
                }
                
        
                // Schließe die Verbindung zum Server
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Fehler: " + e.Message);
            }
        }
    }


}
