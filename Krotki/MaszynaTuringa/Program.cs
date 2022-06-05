using System;
using System.IO;

namespace MaszynaTuringa
{
    public static class TuringHelper
    {
        public static void ConsoleWriteLine(this Turing turing)
        {
            Console.WriteLine("Stan głowicy: " + turing.StanGłowicy);
            Console.WriteLine("Taśma: " + new string(turing.Taśma));
            Console.WriteLine("Program: ");
            foreach (string linia in turing.Program) Console.WriteLine(linia);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //string[] kodProgramu = File.ReadAllLines("program.txt");
            //string taĹma = File.ReadAllText("taĹma.txt");

            string[] kodProgramu =
            {
                "aABb",
                "bBRb",
                "bAAa"
            };
            string taśma = "aAABCAD";

            Console.WriteLine("Początkowy stan maszyny: " + taśma);
            Console.WriteLine("Program: ");
            foreach (string linia in kodProgramu) Console.WriteLine(linia);

            Turing turing = new Turing(taśma, kodProgramu);
            //TuringHelper.ConsoleWriteLine(turing);
            turing.ConsoleWriteLine();

            turing.Run();
            //Console.WriteLine(turing.Output.Length.ToString());
            Console.WriteLine("\nWynik działania programu: ");
            foreach (string s in turing.Output)
                Console.WriteLine(s);
        }
    }
}
