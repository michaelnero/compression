using System;
using System.IO;
using System.Text;

namespace Compression.Compressors
{
    class None : ICompressor
    {
        public ArraySegment<byte> Compress(double[] values)
        {
            using (var memory = new MemoryStream())
            {
                using (var binary = new BinaryWriter(memory, Encoding.UTF8, true))
                {
                    // Version
                    binary.Write((byte) 0);

                    // Values
                    binary.Write(values.Length);
                    for (int i = 0, count = values.Length; i < count; i++)
                    {
                        binary.Write(values[i]);
                    }
                }

                return new ArraySegment<byte>(memory.GetBuffer(), 0, (int) memory.Length);
            }
        }

        public double[] Decompress(ArraySegment<byte> bytes)
        {
            double[] values;

            using (var memory = new MemoryStream(bytes.Array, bytes.Offset, bytes.Count))
            using (var binary = new BinaryReader(memory, Encoding.UTF8))
            {
                // Version
                var version = binary.ReadByte();

                // Values
                var count = binary.ReadInt32();
                values = new double[count];

                for (var i = 0; i < count; i++)
                {
                    values[i] = binary.ReadDouble();
                }
            }

            return values;
        }
    }
}