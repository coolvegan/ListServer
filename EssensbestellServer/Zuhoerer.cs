﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace EssensbestellServer
{
	public class Zuhoerer
	{
        private Queue<String> warteschlage;
        public Zuhoerer(Queue<String> warteSchlange)
        {
            this.warteschlage = warteSchlange;
        }

        public void start()
		{
            // IP-Adresse und Portnummer, an die der Server gebunden werden soll
            string ipAddress = "127.0.0.1"; // Loopback-Adresse für lokale Tests
            int port = 12345; // Portnummer, die du verwenden möchtest

            // Erstelle eine Endpunkt-Adresse, an die der Server gebunden wird
            IPAddress localAddr = IPAddress.Parse(ipAddress);
            IPEndPoint endPoint = new IPEndPoint(localAddr, port);

            // Erstelle einen TCP-Listener
            TcpListener listener = new TcpListener(endPoint);

            try
            {
                // Starte den Listener
                listener.Start();
                Console.WriteLine("Server gestartet. Warte auf Verbindungen...");

                while (true)
                {
                    // Warte auf eine eingehende Verbindung
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine("Client verbunden.");


           
                    NetworkStream stream = client.GetStream();

                    // Puffer für eingehende Daten
                    byte[] buffer = new byte[1024];
                    int bytesRead;

                    // Warte auf und lese eingehende Daten vom Client
                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        string nachricht = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        Console.WriteLine("Nachricht vom Client: " + nachricht);
                        warteschlage.Enqueue(nachricht);
                    }

                  

                    // Verarbeite die Verbindung in einem separaten Thread (optional)
                    // Hier wird die Verarbeitung in der aktuellen Methode durchgeführt.

                    // Sende eine Bestätigungsnachricht an den Client
                    string message = "Verbindung erfolgreich hergestellt.";
                    byte[] data = Encoding.ASCII.GetBytes(message);
                    stream = client.GetStream();
                    stream.Write(data, 0, data.Length);

                    // Schließe die Verbindung zum Client
                    client.Close();
                    Console.WriteLine("Client getrennt.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler: " + ex.Message);
            }
            finally
            {
                // Schließe den Listener, wenn er nicht mehr benötigt wird
                listener.Stop();
            }
        }
	}

}
