namespace Compression.Experiments
{
    public class Returns : IExperiment
    {
        public double[] ProduceValues(int number)
        {
            var market = new Market();
            var marketValues = market.ProduceValues(number);

            var values = new double[number];
            for (var i = 0; i < number; i++)
            {
                if (0 == i)
                {
                    values[i] = 0;
                }
                else
                {
                    values[i] = (marketValues[i] / marketValues[i - 1]) - 1.0;
                }
            }

            return values;
        }
    }
}