using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Compression.Compressors
{
    public class Masking : ICompressor
    {
        private const int WindowSize = 1000;

        public ArraySegment<byte> Compress(double[] values)
        {
            void Mask(long[] window, int size, BinaryWriter binary)
            {
                var middle = size / 2;

                var mask = 0L;

                for (var b = 0; b <= 63; b++)
                {
                    var set = 0;
                    var unset = 0;

                    for (var i = 0; i < size; i++)
                    {
                        if ((set >= middle) || (unset >= middle)) break;

                        if (0 == (window[i] & (1L << b)))
                        {
                            unset++;
                        }
                        else
                        {
                            set++;
                        }
                    }

                    if (set > unset)
                    {
                        mask |= (1L << b);
                    }
                }

                binary.Write(size);
                binary.Write(mask);

                for (var i = 0; i < size; i++)
                {
                    binary.Write(window[i] ^ mask);
                }
            }

            using (var memory = new MemoryStream())
            {
                using (var deflate = new DeflateStream(memory, CompressionLevel.Optimal, true))
                using (var buffer = new BufferedStream(deflate, 8192))
                using (var binary = new BinaryWriter(buffer, Encoding.UTF8, true))
                {
                    // Version
                    binary.Write((byte)0);

                    // Values
                    binary.Write(values.Length);
                    binary.Write((int) Math.Ceiling(values.Length / (double) WindowSize));

                    var window = new long[WindowSize];
                    var size = 0;

                    for (int i = 0, count = values.Length; i < count; i++)
                    {
                        window[size++] = BitConverter.DoubleToInt64Bits(values[i]);
                        if (size == WindowSize)
                        {
                            Mask(window, size, binary);
                            size = 0;
                        }
                    }

                    if (0 != size)
                    {
                        Mask(window, size, binary);
                    }
                }

                return new ArraySegment<byte>(memory.GetBuffer(), 0, (int)memory.Length);
            }
        }

        public double[] Decompress(ArraySegment<byte> bytes)
        {
            double[] values;

            using (var memory = new MemoryStream(bytes.Array, bytes.Offset, bytes.Count))
            using (var deflate = new DeflateStream(memory, CompressionMode.Decompress))
            using (var binary = new BinaryReader(deflate, Encoding.UTF8))
            {
                // Version
                var version = binary.ReadByte();

                // Values
                var count = binary.ReadInt32();
                values = new double[count];

                var valueIndex = 0;

                var numberOfBatches = binary.ReadInt32();
                for (var i = 0; i < numberOfBatches; i++)
                {
                    var size = binary.ReadInt32();
                    var mask = binary.ReadInt64();

                    for (var j = 0; j < size; j++)
                    {
                        values[valueIndex++] = BitConverter.Int64BitsToDouble(binary.ReadInt64() ^ mask);
                    }
                }
            }

            return values;
        }
    }
}