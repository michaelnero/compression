using System;

namespace Compression.Experiments
{
    public interface IExperiment
    {
        double[] ProduceValues(int number);
    }
}