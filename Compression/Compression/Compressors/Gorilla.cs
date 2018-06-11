using System;
using System.Collections.Generic;
using BlueEyes;

namespace Compression.Compressors
{
    public class Gorilla : ICompressor
    {
        public ArraySegment<byte> Compress(double[] values)
        {
            var buffer = new BitBuffer(values.Length * 8);
            var writer = new ValueWriter(buffer);

            for (int i = 0, count = values.Length; i < count; i++)
            {
                writer.AppendValue(values[i]);
            }

            var bytes = buffer.ToArray();
            return new ArraySegment<byte>(bytes);
        }

        public double[] Decompress(ArraySegment<byte> bytes)
        {
            var buffer = new BitBuffer(bytes.ToArray());
            var reader = new ValueReader(buffer);

            var values = new List<double>();
            while (reader.HasMoreValues)
            {
                values.Add(reader.ReadNextValue());
            }

            return values.ToArray();
        }
    }
}