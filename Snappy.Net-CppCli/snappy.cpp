
#include "snappy.h"
#include "exceptions.h"
#include "..\snappy\snappy.h"

namespace Snappy { namespace Net {

    long Snappy::GetUncompressedLength(array<Byte>^ compressed)
    {
        if(compressed == nullptr) {
            throw gcnew ArgumentNullException("compressed");
        }

        pin_ptr<Byte> lpInput = &compressed[0];

        size_t outLength;
        if(!snappy::GetUncompressedLength((char*)lpInput, compressed->Length, &outLength)) {
            throw gcnew InvalidSnappyDataException("Compressed length couldn't be parsed from source data");
        }

        if(outLength > Int64::MaxValue) {
            // Corrupt data...or somehow 8EB was compressed in memory. :x
            throw gcnew InvalidSnappyDataException("Uncompressed length is greater than Int64.MaxValue");
        }

        return (long)outLength;
    }

    bool Snappy::IsValidCompressedBuffer(array<Byte>^ compressed)
    {
        if(compressed == nullptr) {
            throw gcnew ArgumentNullException("compressed");
        }

        pin_ptr<Byte> lpInput = &compressed[0];
        return snappy::IsValidCompressedBuffer((char*)lpInput, compressed->Length);
    }

    long Snappy::MaxCompressedLength(long sourceLength)
    {
        if(sourceLength < 0) {
            throw gcnew ArgumentOutOfRangeException("sourceLength");
        }

        auto result = (long)snappy::MaxCompressedLength(sourceLength);
        if(result < 0) {
            throw gcnew SnappyException("Max compressed length is greater than Int64.MaxValue");
        }

        return result;
    }

    array<Byte>^ Snappy::Uncompress(array<Byte>^ compressed)
    {
        if(compressed == nullptr) {
            throw gcnew ArgumentNullException("compressed");
        }

        // Calling our method instead of native will do range and validation checks for us.
        long uncompressedLength = GetUncompressedLength(compressed);

        auto buffer = gcnew array<Byte>(uncompressedLength);
        
        if(!Uncompress(compressed, buffer))
        {
            throw gcnew InvalidSnappyDataException("Failed to uncompress.  The data is likely invalid or corrupt.");
        }

        return buffer;
    }

    bool Snappy::Uncompress(array<Byte>^ compressed, array<Byte>^ uncompressed)
    {
        if(compressed == nullptr) {
            throw gcnew ArgumentNullException("compressed");
        }

        if(uncompressed == nullptr) {
            throw gcnew ArgumentNullException("uncompressed");
        }

        pin_ptr<Byte> lpInput = &compressed[0];
        pin_ptr<Byte> lpOutput = &uncompressed[0];

        return snappy::RawUncompress((char*)lpInput, compressed->Length, (char*)lpOutput);
    }

    array<Byte>^ Snappy::Compress(array<Byte>^ inputData)
    {
        if(inputData == nullptr) {
            throw gcnew ArgumentNullException("inputData");
        }

        // Create a buffer for the 
        long maxCompressedLen = MaxCompressedLength(inputData->Length);
        auto buffer = gcnew array<Byte>(maxCompressedLen);

        long compressedSize = Compress(inputData, buffer);

        auto resultBuf = gcnew array<Byte>(compressedSize);
        Buffer::BlockCopy(buffer, 0, resultBuf, 0, compressedSize);

        return resultBuf;
    }

    long Snappy::Compress(array<Byte>^ inputData, array<Byte>^ outputBuffer) {
        pin_ptr<Byte> lpInput = &inputData[0];
        pin_ptr<Byte> lpOutput = &outputBuffer[0];

        size_t compressedSize;
        snappy::RawCompress((char*)lpInput, inputData->Length, (char*)lpOutput, &compressedSize);

        // This should never be 0.  It will at least be the size of a header.
        if(compressedSize == 0) {
            throw gcnew SnappyException("Compressed failed for unknown reasons!");
        }

        return compressedSize;
    }
}}