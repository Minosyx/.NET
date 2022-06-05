using System;
using System.Collections.Generic;
using System.Text;

namespace MaszynaTuringa
{
    using Czwórki = SortedList<(char stanGłowicy, char wartośćNaTaśmie), (char nowyStanGłowicy, char nowaWartośćNaTaśmie)>;

	public class Turing
	{
		public char[] Taśma { get; private set; }
	    public char StanGłowicy { get; private set; }
        public int PołożenieGłowicy { get; private set; }
        public string[] Program { get; private set; }
        private Czwórki sparsowanyProgram;
        public string[] Output { get; private set; }


        //private static ((int, char) stanGłowicy, string taśma) analizujTaśmęZeStanemGłowicy(string s)
        //{
        //    int położenieGłowicy = -1;
        //    int liczbaMałychLiter = 0;
        //    int liczbaZnakówNiebędacychLiterami = 0;
        //    for (int i = 0; i < s.Length; ++i)
        //    {
        //        if (char.IsLower(s[i]))
        //        {
        //            liczbaMałychLiter++;
        //            położenieGłowicy = i;
        //        }

        //        if (!char.IsLetter(s[i])) liczbaZnakówNiebędacychLiterami++;
        //    }
        //    if (liczbaMałychLiter != 1) throw new Exception("Błędny zapis taśmy. Musi być jeden symboli głowicy");
        //    if (liczbaZnakówNiebędacychLiterami > 0) throw new Exception("Niedozwolone znaki w opisie taśmy");

        //    (int, char) stanGłowicy = (położenieGłowicy, s[położenieGłowicy]);
        //    string taśma = s.Remove(położenieGłowicy, 1);

        //    return (stanGłowicy, taśma);
        //}


        private static (char[] taśma, char stanGłowicy, int położenieGłowicy) analizujTaśmęZeStanemGłowicy(string s)
        {
	        int położenieGłowicy = -1;
	        int liczbaMałychLiter = 0;
	        int liczbaZnakówNiebędącychLiterami = 0;
	        for (int i = 0; i < s.Length; ++i)
	        {
		        if (char.IsLower(s[i]))
                {
                    liczbaMałychLiter++;
			        położenieGłowicy = i;
		        }

                if (!char.IsLetter(s[i])) liczbaZnakówNiebędącychLiterami++;
            }
	        if (liczbaMałychLiter != 1) throw new Exception("Błędny zapis taśmy. Musi być jeden symboli głowicy");
	        if (liczbaZnakówNiebędącychLiterami > 0) throw new Exception("Niedozwolone znaki w opisie taśmy");

	        string taśma = s.Remove(położenieGłowicy, 1);
	        return (taśma.ToCharArray(), s[położenieGłowicy], położenieGłowicy);
        }

        private static bool czyCzwórkaPoprawna(string linia)
        {
	        Func<char, bool> isLowerLetter = (char c) => c >= 'a' && c <= 'z';
	        Func<char, bool> isUpperLetter = (char c) => c >= 'A' && c <= 'Z';
	        return
		        isLowerLetter(linia[0]) &&
		        isUpperLetter(linia[1]) &&
		        isUpperLetter(linia[2]) &&
		        isLowerLetter(linia[3]);
        }

        private static Czwórki parsujProgram(string[] kodProgramu)
        {
	        Czwórki czwórki = new Czwórki();
	        foreach (string linia in kodProgramu)
	        {
		        if (string.IsNullOrWhiteSpace(linia)) continue;
		        if (!czyCzwórkaPoprawna(linia)) throw new Exception("Niepoprawna linia kodu: " + linia);
		        (char, char) bieżącyStan = (linia[0], linia[1]);
		        (char, char) nowyStan = (linia[3], linia[2]);
		        if (czwórki.ContainsKey(bieżącyStan)) throw new Exception("Program nie może zawierać dwóch linii o tych samych dwóch pierwszych literach");
		        czwórki.Add(bieżącyStan, nowyStan);
	        }
	        return czwórki;
        }

        public Turing(string taśmaZeStanemGłowicy, string[] program)
        {
	        var stanMaszyny = analizujTaśmęZeStanemGłowicy(taśmaZeStanemGłowicy);
	        this.StanGłowicy = stanMaszyny.stanGłowicy;
	        this.PołożenieGłowicy = stanMaszyny.położenieGłowicy;
            this.Taśma = stanMaszyny.taśma;
	        this.Program = program;
        }

        public void Run()
        {
	        this.sparsowanyProgram = parsujProgram(Program);
	        List<string> output = wykonajProgram((Taśma, StanGłowicy, PołożenieGłowicy), sparsowanyProgram);
            Output = output.ToArray();
        }

		private static (char nowyStanGłowicy, char nowaWartośćLubPolecenie)? znajdźPolecenie(
			char stanGłowicy, char wartośćNaTaśmie, Czwórki program)
        {
	        (char, char) bieżącyStan = (stanGłowicy, wartośćNaTaśmie);
            if (program.ContainsKey(bieżącyStan)) return program[bieżącyStan];
            else return null;
		}		
		
		private static List<string> wykonajProgram(
			(char[] taśma, char stanGłowicy, int położenieGłowicy) stanMaszyny, Czwórki program)
		{
	        List<string> historia = new List<string>();
	        (char nowyStanGłowicy, char nowaWartośćLubPolecenie)? polecenie;
	        while ((polecenie = znajdźPolecenie(stanMaszyny.stanGłowicy, stanMaszyny.taśma[stanMaszyny.położenieGłowicy], program)) != null)
	        {
		        stanMaszyny.stanGłowicy = polecenie.Value.nowyStanGłowicy;
		        switch (polecenie.Value.nowaWartośćLubPolecenie)
				{
					case 'L':
						stanMaszyny.położenieGłowicy--;
			            //Console.WriteLine("*L");
			            break;
					case 'R':
						stanMaszyny.położenieGłowicy++;
			            //Console.WriteLine("*R");
			            break;
			        default:
						stanMaszyny.taśma[stanMaszyny.położenieGłowicy] = polecenie.Value.nowaWartośćLubPolecenie;
			            //Console.WriteLine("*1" + stanMaszyny.położenieGłowicy);
			            //Console.WriteLine("*1" + polecenie.Value.nowaWartośćLubPolecenie);
			            //Console.WriteLine("*1" + stanMaszyny.taśma[stanMaszyny.położenieGłowicy]);
			            break;
		        }
		    historia.Add(pobierzŁańcuchOpisującyStanMaszyny(stanMaszyny));
		    Console.WriteLine(new string(stanMaszyny.taśma));
	        }
	        return historia;
        }


        private static string pobierzŁańcuchOpisującyStanMaszyny((char[] taśma, char stanGłowicy, int położenieGłowicy) stanMaszyny)
		{
	        string s = new string(stanMaszyny.taśma);
	        //Console.WriteLine("*2" + s);
	        s = s.Insert(stanMaszyny.położenieGłowicy, stanMaszyny.stanGłowicy.ToString());
	        //Console.WriteLine("*3" + s);
	        //Console.WriteLine("*3" + stanMaszyny.poĹoĹźenieGĹowicy);
	        //Console.WriteLine("*3" + stanMaszyny.stanGĹowicy);
	        return s;
        }
    }
}
