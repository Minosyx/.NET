using Microsoft.VisualStudio.TestTools.UnitTesting;
using SettingsManagerProject;
using System;

namespace TestSettingsManager
{
    [TestClass]
    public class TestSettingsManager
    {
        class Samoch�d
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
            Samoch�d samoch�d = new Samoch�d() { Marka = "Tesla", Model = 1 };
            SettingsManager<Samoch�d> sm = new SettingsManager<Samoch�d>(samoch�d, "s.xml");
            //sm.Save();
            //SettingsManager<Samoch�d> sm1 = SettingsManager<Samoch�d>.Load(path)
            //sm1.GetSettingsObject() =? samoch�d;
        }
    }
}