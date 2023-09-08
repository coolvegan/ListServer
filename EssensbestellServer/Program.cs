using System;
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


        Queue<String> nachrichten = new Queue<string>();


        // Starten Sie eine Task und übergeben Sie den Parameter
        Task task = Task.Run(() => DoWork(nachrichten));
        Zuhoerer zuhoerer = new Zuhoerer(nachrichten);
        zuhoerer.start();

      


    }

    static void DoWork(Queue<String> nachrichten) {
        while (true)
        {
            Console.WriteLine("Es existieren in der Queue " + nachrichten.Count + " Nachrichten.");
            if (nachrichten.Count == 0)
            {
                Task.Delay(2000).Wait();
                DoWork(nachrichten);
                return;
            }

            String nachricht;
            lock (nachrichten)
            {
                nachricht = nachrichten.Dequeue();
            }
            // Deserialisieren Sie den JSON-String in ein JSON-Objekt
            // Deserialisieren Sie den JSON-String in ein JSON-Objekt
            JsonDocument jsonDoc = JsonDocument.Parse(nachricht);


            // Zugriff auf Werte im JSON-Objekt
            JsonElement root = jsonDoc.RootElement;
            string name = root.GetProperty("Name").GetString();
            int age = root.GetProperty("Age").GetInt32();
            string city = root.GetProperty("City").GetString();
            Console.WriteLine("Marco Kittel ist der beste!");
            Console.WriteLine($"Name: {name}");
            Console.WriteLine($"Age: {age}");
            Console.WriteLine($"City: {city}");
        }
  
    }
}
