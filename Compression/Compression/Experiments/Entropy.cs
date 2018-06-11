using System;

namespace Compression.Experiments
{
    public class Entropy : IExperiment
    {
        public double[] ProduceValues(int number)
        {
            var random = new Random();

            var values = new double[number];
            for (var i = 0; i < number; i++)
            {
                values[i] = random.NextDouble();
            }

            return values;
        }
    }
}