using Microsoft.VisualStudio.TestTools.UnitTesting;
using SettingsManagerProject;
using System;

namespace TestSettingsManager
{
    [TestClass]
    public class TestSettingsManager
    {
        class Samochód
        {
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
        }

        [TestMethod]
        public void TestMethod1()
        {
            Samochód samochód = new Samochód() { Marka = "Tesla", Model = 1 };
            SettingsManager<Samochód> sm = new SettingsManager<Samochód>(samochód, "s.xml");
            //sm.Save();
            //SettingsManager<Samochód> sm1 = SettingsManager<Samochód>.Load(path)
            //sm1.GetSettingsObject() =? samochód;
        }
    }
}