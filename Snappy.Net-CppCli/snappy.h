// Snappy.Net-CppCli.h

#pragma once

using namespace System;

namespace Snappy { namespace Net {

    public ref class Snappy abstract sealed
    {
    public:
        static long GetUncompressedLength(array<Byte>^ compressed);
        static bool IsValidCompressedBuffer(array<Byte>^ compressed);
        static long MaxCompressedLength(long sourceLength);
        static array<Byte>^ Uncompress(array<Byte>^ compressed);
        static bool Uncompress(array<Byte>^ compressed, array<Byte>^ uncompressed);
        static array<Byte>^ Compress(array<Byte>^ input);
    private:
        static long Compress(array<Byte>^ input, array<Byte>^ buffer);
    };
}}