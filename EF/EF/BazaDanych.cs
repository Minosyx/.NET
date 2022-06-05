using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EF
{
    public class BazaDanychOsóbDbContext : DbContext
    {
        public DbSet<Osoba> Osoby { get; set; }
        public DbSet<Adres> Adresy { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=osoby.db");
        }
    }

    public class BazaDanychOsób : IDisposable
    {
        private BazaDanychOsóbDbContext dbc = new BazaDanychOsóbDbContext();

        public BazaDanychOsób()
        {
            dbc.Database.EnsureCreated();
        }

        public void Dispose()
        {
            dbc.Dispose();
        }

#if DEBUG
        public Osoba[] Osoby => dbc.Osoby.ToArray();
        public Adres[] Adresy => dbc.Adresy.ToArray();
#endif

        public Osoba? PobierzOsobę(int id)
        {
            return dbc.Osoby.FirstOrDefault(o => o.Id == id);
        }

        public Osoba? this[int id]
        {
            get => PobierzOsobę(id);
            set => ZmieńOsobę(id, value);
        }

        public int[]? identyfikatoryOsób => dbc.Osoby.Select(o => o.Id).ToArray();

        public int DodajOsobę(Osoba osoba)
        {
            if (osoba == null) 
                throw new ArgumentNullException(nameof(osoba), "Podano pustą referencję jako argument");
            if (osoba.Adres == null)
                throw new ArgumentException("Brak adresu!", nameof(osoba.Adres));

            if (dbc.Osoby.ToArray().Any(o => o.Equals(osoba))) return osoba.Id;
            else
            {
                if (dbc.Osoby.Any(o => o.Id == osoba.Id))
                    osoba.Id = dbc.Osoby.Max(o => o.Id) + 1;
            }

            Adres? adres = dbc.Adresy.ToArray().FirstOrDefault(a => a.Equals(osoba.Adres));
            if (adres != null) osoba.Adres = adres;

            dbc.Osoby.Add(osoba);
            dbc.SaveChanges();

            return osoba.Id;
        }

        private int[] pobierzIdentyfikatoryUżywanychAdresów()
        {
            return dbc.Osoby.Select(o => o.Adres.Id).Distinct().ToArray();
        }

        private Adres[] pobierzNieużywaneAdresy()
        {
            int[] używaneIdentyfikatoryAdresów = pobierzIdentyfikatoryUżywanychAdresów();
            List<Adres> nieużywaneAdresy = new List<Adres>();
            foreach (Adres adres in dbc.Adresy)
            {
                if (!używaneIdentyfikatoryAdresów.Contains(adres.Id))
                    nieużywaneAdresy.Add(adres);
            }

            return nieużywaneAdresy.ToArray();
        }

        private void usuńNieużywaneAdresy()
        {
            dbc.Adresy.RemoveRange(pobierzNieużywaneAdresy());
            dbc.SaveChanges();
        }

        public void UsuńOsobę(int idOsoby)
        {
            Osoba? os = PobierzOsobę(idOsoby);
            if (os != null)
            {
                dbc.Osoby.Remove(os);
                dbc.SaveChanges();
                usuńNieużywaneAdresy();
            }
        }

        public void ZmieńOsobę(int idOsoby, Osoba? os)
        {
            Osoba? o = PobierzOsobę(idOsoby);
            if (o == null)
                throw new Exception("Nie ma osoby o podanym identyfikatorze!");

            if (os == null)
                throw new Exception("Brak danych!");

 
            o.Imię = os.Imię ?? o.Imię;
            o.Adres = os.Adres ?? o.Adres;
            o.Nazwisko = os.Nazwisko ?? o.Nazwisko;
            if (os.NumerTelefonu != 0) o.NumerTelefonu = os.NumerTelefonu ?? o.NumerTelefonu;
            dbc.SaveChanges();
        }
    }
}
