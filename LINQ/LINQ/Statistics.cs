using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

static class Statistics
{
    public static double StdDev(this IEnumerable<double> values)
    {
        double average = values.Average();
        double result = 0;
        {
            double dev;
            foreach (double value in values)
            {
                dev = value - average;
                result += dev * dev;
            }
        }
        return Math.Sqrt(result / values.Count());
    }

    public static double StdDev<T>(this IEnumerable<T> values, Func<T, double> selector)
    {
        double average = 0;
        foreach (T value in values)
            average += selector(value);
        average /= values.Count();

        double result = 0;
        {
            double dev;
            foreach (T value in values)
            {
                dev = selector(value) - average;
                result += dev * dev;
            }
        }
        return Math.Sqrt(result / values.Count());
    }

    public static IEnumerable<T> DiscardThickErrors<T>(this IEnumerable<T> values, Func<T, double> selector)
    {
        double stddev = StdDev(values, selector);
        double average = values.Average(selector);
        List<T> discarded = new List<T>();
        foreach (T value in values)
            if (selector(value) - average < 2 * stddev)
                discarded.Add(value);
        return discarded;
    }
}

