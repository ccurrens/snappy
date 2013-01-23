#include "snappy.h"

#define DllExport extern "C" __declspec( dllexport )

namespace snappydll {
    DllExport bool GetUncompressedLength(const char* compressed, size_t compressed_length, size_t* result)
    {
        return snappy::GetUncompressedLength(compressed, compressed_length, result);
    }

    DllExport bool IsValidCompressedBuffer(const char* compressed, size_t compressed_length)
    {
        return snappy::IsValidCompressedBuffer(compressed, compressed_length);
    }

    DllExport size_t MaxCompressedLength(size_t source_bytes)
    {
        return snappy::MaxCompressedLength(source_bytes);
    }

    DllExport bool RawUncompress(const char* compressed, size_t compressed_length, char* uncompressed)
    {
        return snappy::RawUncompress(compressed, compressed_length, uncompressed);
    }

    DllExport void RawCompress(const char* input, size_t input_length, char* compressed, size_t* compressed_length)
    {
        return snappy::RawCompress(input, input_length, compressed, compressed_length);
    }
}