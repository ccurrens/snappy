using System;

namespace Snappy.Net
{
    public static class Snappy
    {
        public static ulong GetUncompressedLength(byte[] compressed)
        {
            if (compressed == null)
            {
                throw new ArgumentNullException("compressed");
            }

            ulong result;
            if (!NativeMethods.GetUncompressedLength(compressed, (ulong)compressed.Length, out result))
            {
                throw new InvalidSnappyDataException();
            }
            
            return result;
        }

        public static bool IsValidCompressedBuffer(byte[] compressed)
        {
            if (compressed == null)
            {
                throw new ArgumentNullException("compressed");
            }

            var isComp = NativeMethods.IsValidCompressedBuffer(compressed, (ulong)compressed.Length);
            return isComp;
        }

        public static int MaxCompressedLength(int sourceLength)
        {
            if (sourceLength < 0)
            {
                throw new ArgumentOutOfRangeException("sourceLength");
            }

            var result = MaxCompressedLength((long) sourceLength);

            if (result > int.MaxValue)
            {
                throw new SnappyException("Max compressed length is greater than Int32.MaxValue.  Use the long overload instead.");
            }

            return (int) result;
        }

        public static long MaxCompressedLength(long sourceLength)
        {
            if (sourceLength < 0)
            {
                throw new ArgumentOutOfRangeException("sourceLength");
            }

            try
            {
                return checked(32 + sourceLength + (sourceLength / 6));
            }
            catch (OverflowException)
            {
                // Hitting an overflow is likely programmer's error...it would be a massive byte[] allocation.
                throw new SnappyException("Max compressed length is greater than Int64.MaxValue.");
            }
        }

        public static byte[] RawUncompress(byte[] compressed)
        {
            if (compressed == null)
            {
                throw new ArgumentNullException("compressed");
            }
            
            var uncompressedLength = GetUncompressedLength(compressed);
            var buf = new byte[uncompressedLength];

            if (!RawUncompress(compressed, buf))
            {
                throw new InvalidSnappyDataException();
            }

            return buf;
        }

        public static bool RawUncompress(byte[] compressed, byte[] uncompressed)
        {
            if (compressed == null)
            {
                throw new ArgumentNullException("compressed");
            }

            if (uncompressed == null)
            {
                throw new ArgumentNullException("uncompressed");
            }

            return NativeMethods.RawUncompress(compressed, (ulong)compressed.Length, uncompressed);
        }

        public static byte[] RawCompress(byte[] input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            ulong compLen;

            var maxCompLen = MaxCompressedLength(input.Length);
            var compBuffer = new byte[maxCompLen];

            NativeMethods.RawCompress(input, (ulong)input.Length, compBuffer, out compLen);

            if (compLen == 0)
            {
                throw new SnappyException("Compression failed for unknown reasons.");
            }
            
            // TODO: Do this right, with long instead of int casts.  But who is going to compress > 2GB of data in memory...well at least now?
            var b = new byte[compLen];
            Buffer.BlockCopy(compBuffer, 0, b, 0, (int)compLen);

            return b;
        }
    }
}