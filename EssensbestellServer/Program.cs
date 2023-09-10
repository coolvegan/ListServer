using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json;
using EssensbestellServer;

class TCPServer
{
    static void Main()
    {
        Queue<Tuple<String, TcpClient>> nachrichten = new Queue<Tuple<string, TcpClient>>();

        // Starten Sie eine Task und übergeben Sie den Parameter
        Task task = Task.Run(() => DoWork(nachrichten));
        Zuhoerer zuhoerer = new Zuhoerer(nachrichten);
        zuhoerer.start();

    }

    static void DoWork(Queue<Tuple<String, TcpClient>> nachrichten) {
        while (true)
        {
            bool isJson = true;
            Console.WriteLine("Es existieren in der Queue " + nachrichten.Count + " Nachrichten.");
            if (nachrichten.Count == 0)
            {
                Task.Delay(2000).Wait();
                DoWork(nachrichten);
                return;
            }

            Tuple<string, TcpClient> nachricht;
            lock (nachrichten)
            {
                nachricht = nachrichten.Dequeue();
            }
            // Deserialisieren Sie den JSON-String in ein JSON-Objekt
            // Deserialisieren Sie den JSON-String in ein JSON-Objekt
            try
            {
                JsonNachricht jsonNachricht = new JsonNachricht();
                jsonNachricht.ausgabe(nachricht);
                if (Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    nachricht.Item2.Close();
                }
            }

            catch (Exception ex)
            {

            }


            

        }

    }
}
