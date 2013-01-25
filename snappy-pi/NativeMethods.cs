// Copyright (c) Christopher Currens.  Licensed under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0.html)

using System;
using System.Runtime.InteropServices;

namespace Snappy.Net
{
    internal static class NativeMethods
    {
        /************************************************
         * 32-bit signatures
         ************************************************/
        [DllImport("snappy.dll", EntryPoint = "GetUncompressedLength", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)] // Prevents the runtime from marshalling 0xcccccc00 as true instead of false
        private static extern bool GetUncompressedLength32(byte[] compressed, uint compressedLength, out uint result);

        [DllImport("snappy.dll", EntryPoint = "IsValidCompressedBuffer", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool IsValidCompressedBuffer32(byte[] compressed, uint compressedLength);

        [DllImport("snappy.dll", EntryPoint = "RawUncompress", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool RawUncompress32(byte[] compressed, uint compressedLength, byte[] uncompressed);

        [DllImport("snappy.dll", EntryPoint = "RawCompress", CallingConvention = CallingConvention.Cdecl)]
        private static extern void RawCompress32(byte[] input, uint inputLength, byte[] compressed, out uint compressedLength);

        /************************************************
         * 64-bit signatures
         ************************************************/
        [DllImport("snappy.dll", EntryPoint = "GetUncompressedLength", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)] // Prevents the runtime from marshalling 0xcccccc00 as true instead of false
        private static extern bool GetUncompressedLength64(byte[] compressed, ulong compressedLength, out ulong result);

        [DllImport("snappy.dll", EntryPoint = "IsValidCompressedBuffer", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool IsValidCompressedBuffer64(byte[] compressed, ulong compressedLength);

        [DllImport("snappy.dll", EntryPoint = "RawUncompress", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool RawUncompress64(byte[] compressed, ulong compressedLength, byte[] uncompressed);

        [DllImport("snappy.dll", EntryPoint = "RawCompress", CallingConvention = CallingConvention.Cdecl)]
        private static extern void RawCompress64(byte[] input, ulong inputLength, byte[] compressed, out ulong compressedLength);

        public static bool GetUncompressedLength(byte[] compressed, ulong compressedLength, out ulong result)
        {
            if (Environment.Is64BitProcess)
            {
                return GetUncompressedLength64(compressed, compressedLength, out result);
            }

            if (compressedLength > uint.MaxValue)
            {
                throw new ArgumentOutOfRangeException("compressedLength",
                                                      "Cannot get uncompressed length from a compressed length greater than UInt32.MaxValue on a 32 bit system");
            }

            uint result32;
            var compressedLength32 = (uint) compressedLength;
            var success = GetUncompressedLength32(compressed, compressedLength32, out result32);

            result = result32;
            return success;
        }

        public static bool IsValidCompressedBuffer(byte[] compressed, ulong compressedLength)
        {
            if (Environment.Is64BitProcess)
            {
                return IsValidCompressedBuffer64(compressed, compressedLength);
            }

            if (compressedLength > uint.MaxValue)
            {
                throw new ArgumentOutOfRangeException("compressedLength",
                                                      "Cannot process data with a length greater than UInt32.MaxValue on a 32 bit system");
            }

            return IsValidCompressedBuffer32(compressed, (uint)compressedLength);
        }

        public static bool RawUncompress(byte[] compressed, ulong compressedLength, byte[] uncompressed)
        {
            if (Environment.Is64BitProcess)
            {
                return RawUncompress64(compressed, compressedLength, uncompressed);
            }

            if (compressedLength > uint.MaxValue)
            {
                throw new ArgumentOutOfRangeException("compressedLength",
                                                      "Cannot process data with a length greater than UInt32.MaxValue on a 32 bit system");
            }

            return RawUncompress32(compressed, (uint)compressedLength, uncompressed);
        }

        public static void RawCompress(byte[] input, ulong inputLength, byte[] compressed, out ulong compressedLength)
        {
            if (Environment.Is64BitProcess)
            {
                RawCompress64(input, inputLength, compressed, out compressedLength);
            }
            else
            {
                if (inputLength > uint.MaxValue)
                {
                    throw new ArgumentOutOfRangeException("compressedLength",
                                                          "Cannot process data with a length greater than UInt32.MaxValue on a 32 bit system");
                }

                uint compressedLength32;
                RawCompress32(input, (uint) inputLength, compressed, out compressedLength32);
                compressedLength = compressedLength32;
            }
        }
    }
}
