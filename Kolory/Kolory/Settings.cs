using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Globalization;

namespace Kolory;

static class Settings
{
    private static string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "kolory.xml");
    private static IFormatProvider formatProvider = CultureInfo.InvariantCulture;
    public static void Save(double r, double g, double b)
    {
        XDocument xml = new XDocument(
            new XComment("Zapisano: " + DateTime.Now),
            new XElement("ustawienia",
                new XElement("r", r.ToString(formatProvider)),
                new XElement("g", g.ToString(formatProvider)),
                new XElement("b", b.ToString(formatProvider))
            )
        );
        xml.Save(filePath);
    }

    public static (double r, double g, double b) Load()
    {
        if (!File.Exists(filePath)) return (0.0, 0.0, 0.0);
        XDocument xml = XDocument.Load(filePath);
        double r = double.Parse(xml.Root.Element("r").Value, formatProvider);
        double g = double.Parse(xml.Root.Element("g").Value, formatProvider);
        double b = double.Parse(xml.Root.Element("b").Value, formatProvider);
        return (r, g, b);
    }
}

