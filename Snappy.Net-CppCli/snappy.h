// Snappy.Net-CppCli.h

#pragma once

using namespace System;

namespace Snappy { namespace Net {

    public ref class Snappy
    {
    public:
        long GetUncompressedLength(array<Byte>^ compressed);
        bool IsValidCompressedBuffer(array<Byte>^ compressed);
        long MaxCompressedLength(long sourceLength);
        array<Byte>^ Uncompress(array<Byte>^ compressed);
        bool Uncompress(array<Byte>^ compressed, array<Byte>^ uncompressed);
        array<Byte>^ Compress(array<Byte>^ input);
    };
}}