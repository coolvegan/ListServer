using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Threading;  // Für Multi-Threading

namespace EssensbestellServer
{
    public class Zuhoerer
    {
        private Queue<Tuple<String, TcpClient>> warteschlage;
        private static List<Essenstermin> Essenstermine = new List<Essenstermin>();

        public Zuhoerer(Queue<Tuple<String, TcpClient>> warteSchlange)
        {
            this.warteschlage = warteSchlange;
        }

        public void TerminHinzufuegen(Essenstermin termin)
        {
            Essenstermine.Add(termin);
        }

        public void TerminLoeschen(DateTime datum)
        {
            Essenstermine.RemoveAll(t => t.Datum == datum);
        }

        public void TerminAktualisieren(Essenstermin alterTermin, Essenstermin neuerTermin)
        {
            int index = Essenstermine.IndexOf(alterTermin);
            if (index != -1)
            {
                Essenstermine[index] = neuerTermin;
            }
        }

        public List<Essenstermin> AlleTermineAbrufen()
        {
            return Essenstermine;
        }

        private void HandleClient(TcpClient client)
        {
            Console.WriteLine("Client verbunden in Thread " + Thread.CurrentThread.ManagedThreadId);
            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[1024];
            int bytesRead;

            try
            {
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string nachricht = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    warteschlage.Enqueue(new Tuple<string, TcpClient>(nachricht.Trim(), client));
                    Console.WriteLine("Nachricht vom Client: " + nachricht);

                    // Die Nachrichtenverarbeitung, die Sie bereits im Code hatten, bleibt hier.
                    // ...
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler beim Kommunizieren mit Client: " + ex.Message);
            }
            finally
            {
                client.Close();
                Console.WriteLine("Client getrennt in Thread " + Thread.CurrentThread.ManagedThreadId);
            }
        }

        public void start()
        {
            string ipAddress = "127.0.0.1";
            int port = 12345;

            IPAddress localAddr = IPAddress.Parse(ipAddress);
            IPEndPoint endPoint = new IPEndPoint(localAddr, port);
            TcpListener listener = new TcpListener(endPoint);

            try
            {
                listener.Start();
                Console.WriteLine("Server gestartet. Warte auf Verbindungen...");

                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();

                    // Starten Sie einen neuen Thread für jeden Client
                    Thread clientThread = new Thread(() => HandleClient(client));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler: " + ex.Message);
            }
            finally
            {
                listener.Stop();
            }
        }
    }
}
