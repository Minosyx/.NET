using System;

namespace Krotki
{
    class Program
    {
        static void Main(string[] args)
        {
            ValueTuple<int, double, string> a1 = new ValueTuple<int, double, string>(1, Math.PI, "UMK");
            Console.WriteLine(a1);

            (int, double, string) a2 = (1, Math.PI, "UMK");
            Console.WriteLine(a2);

            Console.WriteLine(a2.Item1);
            Console.WriteLine(a2.Item2);
            Console.WriteLine(a2.Item3);

            (int liczbaCałkowita, double liczbaRzeczywista, string łańuch) a3 = (1, Math.PI, "UMK");
            Console.WriteLine(a3);

            Console.WriteLine(a3.liczbaCałkowita);
            Console.WriteLine(a3.liczbaRzeczywista);
            Console.WriteLine(a3.łańuch);

            Console.WriteLine(jakaśMetoda());
        }

        private static (int liczba1, double liczba2, string łańcuch) jakaśMetoda()
        {
            return (1, Math.PI, "UMK");
        }
    }
}
