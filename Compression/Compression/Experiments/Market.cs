using System;

namespace Compression.Experiments
{
    public class Market : IExperiment
    {
        public double[] ProduceValues(int number)
        {
            var random = new Random();
            var seed = random.Next(1, 100_000) + Math.Round(random.NextDouble(), 4);

            var values = new double[number];
            for (var i = 0; i < number; i++)
            {
                var sign = (1 == (int)Math.Round(random.NextDouble())) ? 1 : -1;
                var drift = random.NextDouble();
                var value = Math.Round(seed + (drift * sign), 4);

                values[i] = value;
            }

            return values;
        }
    }
}