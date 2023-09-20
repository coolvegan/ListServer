using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;

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
                    Console.WriteLine("Client verbunden.");
                    NetworkStream stream = client.GetStream();

                    byte[] buffer = new byte[1024];
                    int bytesRead;

                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        string nachricht = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        warteschlage.Enqueue(new Tuple<string, TcpClient>(nachricht.Trim(), client));
                        Console.WriteLine("Nachricht vom Client: " + nachricht);

                        if (nachricht.StartsWith("ADD:"))
                        {
                            string[] teile = nachricht.Substring(4).Split(';');
                            if (teile.Length >= 3)
                            {
                                DateTime datum = DateTime.Parse(teile[0]);
                                string beschreibung = teile[1];
                                TimeSpan uhrzeit = TimeSpan.Parse(teile[2]);
                                Essenstermin neuerTermin = new Essenstermin(datum, beschreibung, uhrzeit);
                                TerminHinzufuegen(neuerTermin);
                                string antwort = "Termin erfolgreich hinzugefügt!";
                                byte[] antwortBytes = Encoding.ASCII.GetBytes(antwort);
                                stream.Write(antwortBytes, 0, antwortBytes.Length);
                            }
                        }
                        else if (nachricht.StartsWith("DELETE:"))
                        {
                            DateTime datum = DateTime.Parse(nachricht.Substring(7));
                            TerminLoeschen(datum);
                            string antwort = "Termin erfolgreich gelöscht!";
                            byte[] antwortBytes = Encoding.ASCII.GetBytes(antwort);
                            stream.Write(antwortBytes, 0, antwortBytes.Length);
                        }
                        // Weiter Interpretationen für andere Nachrichten können hier hinzugefügt werden
                    }
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
