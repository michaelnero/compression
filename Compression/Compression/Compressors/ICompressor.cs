using System;

namespace Compression.Compressors
{
    public interface ICompressor
    {
        ArraySegment<byte> Compress(double[] values);

        double[] Decompress(ArraySegment<byte> bytes);
    }
}