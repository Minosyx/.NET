using System;

namespace AsyncAwait
{
    class Program
    {
        static void ConsoleWriteLine(string s)
        {
            int id = System.Threading.Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine(id.ToString() + " " + s);
        }

        static void Main(string[] args)
        {
            AsyncCoding();
            ConsoleWriteLine("Naciśnij ENTER.....");
            Console.ReadLine();
        }

        static async void AsyncCoding()
        {
            Task<long> zadanie = DoAsync("zadanie-metoda");
            ConsoleWriteLine("Zadanie zostało uruchomione");
            //long wynik = zadanie.Result;
            long wynik = await zadanie;
            ConsoleWriteLine("Zadanie: " + wynik.ToString());
        }

        static Task<long> DoAsync(object parameter)
        {
            Func<object, long> akcja = (object parametr) =>
            {
                ConsoleWriteLine("Początek działania akcji - " + parametr.ToString());
                Thread.Sleep(500);
                ConsoleWriteLine("Koniec działania akcji - " + parametr.ToString());
                return DateTime.Now.Ticks;
            };

            //long wynik = akcja("synchronicznie");
            //Console.WriteLine("Synchornicznie " + wynik.ToString());

            Task<long> zadanie = new Task<long>(akcja, "zadanie");
            zadanie.Start();
            return zadanie;
        }
    }
}

