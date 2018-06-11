using System;

namespace Compression.Compressors
{
    static class Helpers
    {
        public static bool AreEqual(double[] left, double[] right, double tolerance = double.Epsilon)
        {
            if ((null == left) || (null == right)) return false;
            if (left.Length != right.Length) return false;

            for (int i = 0, count = left.Length; i < count; i++)
            {
                if (Math.Abs(left[i] - right[i]) > tolerance)
                {
                    return false;
                }
            }

            return true;
        }
    }
}