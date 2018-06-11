using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Compression.Compressors;
using Compression.Experiments;

namespace Compression
{
    class Program
    {
        static void Main(string[] args)
        {
            var experiments = new List<IExperiment>
            {
                new Entropy(),
                new Market(),
                new Returns()
            };

            var compressors = new List<ICompressor>
            {
                new None(),
                new NonOptimized(),
                new Delta(),
                new Scaling(),
                new Shuffling(),
                new Masking(),
                new Gorilla(),
            };

            foreach (var experiment in experiments)
            {
                for (var number = 365; number <= 365 * 20; number += 365)
                {
                    var values = experiment.ProduceValues(number);

                    var baselineBytes = 0;
                    foreach (var compressor in compressors)
                    {
                        var byteCount = 0;
                        var time = Time(10, () =>
                        {
                            var temp = compressor.Compress(values);
                            byteCount = temp.Count;
                        });

                        if (compressor is None)
                        {
                            baselineBytes = byteCount;
                        }

                        var rate = Math.Round(((baselineBytes * 1000.0) / time) / 1_000_000.0, 2);

                        Console.WriteLine($"{experiment.GetType().Name},{compressor.GetType().Name},{number},{byteCount},{time},{rate}");
                    }
                }
            }
        }

        public static double Time(int count, Action action)
        {
            count = count + 1;

            var times = new List<double>(count);

            for (var i = 0; i < count; i++)
            {
                var timer = Stopwatch.StartNew();
                action();
                timer.Stop();

                var ticks = (double) timer.ElapsedTicks;
                var milliseconds = (ticks / Stopwatch.Frequency) * 1000.0;

                times.Add(milliseconds);
            }

            return times.Skip(1).Average();
        }
    }
}
