// Copyright (c) Christopher Currens.  Licensed under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0.html)

#include "snappy.h"

#define DllExport extern "C" __declspec(dllexport)

namespace snappydll
{
    DllExport bool GetUncompressedLength(const char* compressed, size_t compressed_length, size_t* result)
    {
        return snappy::GetUncompressedLength(compressed, compressed_length, result);
    }

    DllExport bool IsValidCompressedBuffer(const char* compressed, size_t compressed_length)
    {
       auto res = snappy::IsValidCompressedBuffer(compressed, compressed_length);
       return res;
    }

    DllExport size_t MaxCompressedLength(size_t source_len)
    {
        return snappy::MaxCompressedLength(source_len);
    }

    DllExport bool RawUncompress(const char* compressed, size_t compressed_length, char* uncompressed)
    {
        return snappy::RawUncompress(compressed, compressed_length, uncompressed);
    }

    DllExport void RawCompress(const char* input, size_t input_length, char* compressed, size_t* compressed_length)
    {
        snappy::RawCompress(input, input_length, compressed, compressed_length);
    }
}