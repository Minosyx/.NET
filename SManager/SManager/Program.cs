using SettingsManagerProject;

namespace SManager
{
    class Program
    {
        public struct Struktura
        {
            public string lancuch;
            public double liczba;
        }
        class TClass
        {
            public double liczba;
            public Struktura s;
            public TClass()
            {
                liczba = 2;
                this.s = new Struktura();
                s.liczba = 6;
                s.lancuch = "test";
            }
        }
        class Samochód
        {
            public TClass Klasa;
            public Samochód()
            {
                this.marka = "trabant";
                this.Klasa = new TClass();
            }

            public Samochód(string marka)
            {
                this.marka = marka;
                this.Klasa = new TClass();
            }

            private string marka;

            public string Marka
            {
                get
                {
                    return marka;
                }
                set
                {
                    marka = value;
                }
            }

            public int Model;
            [NonSerialized] public int Podmodel;

            public int[] Tablica = { 1, 2, 3 };

            public enum _TypWyliczeniowy { Raz, Dwa, Trzy }
            public _TypWyliczeniowy TypWyliczeniowy = Samochód._TypWyliczeniowy.Dwa;
        }

        static void Main(string[] args)
        {
            Samochód samochód = new Samochód("Tesla") { Model = 1 };
            SettingsManager<Samochód> sm = new SettingsManager<Samochód>(samochód, "s.xml");
            sm.Save();

            //System.IO.File.Copy("s.xml", "r.xml", true);
            //SettingsManager<Samochód> kopia = SettingsManager<Samochód>.Load("r.xml");
            //Samochód skopia = kopia.GetSettingsObject();
            //bool wynik = samochód.Equals(skopia);
            //Console.WriteLine(wynik);
        }
    }
}