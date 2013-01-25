
using namespace System;
using namespace System::Diagnostics::CodeAnalysis;
using namespace System::Runtime::Serialization;

namespace Snappy { namespace Net {
    /// <summary>
    /// The base class for all exceptions directly related to snappy
    /// </summary>
    [Serializable]
    public ref class SnappyException : Exception
    {
    public:
        SnappyException() {}
        SnappyException(String^ message) : Exception(message) { }
        SnappyException(String^ message, Exception^ inner) : Exception(message, inner) {}
    protected:
        SnappyException(SerializationInfo^ info, StreamingContext context) : Exception(info, context) {}
    };

    /// <summary>
    /// The exception thrown when data that has been compressed with snappy fails to
    /// uncompress, or is otherwise invalid or corrupt.
    /// </summary>
    [Serializable]
    public ref class InvalidSnappyDataException : SnappyException
    {
    public:
        InvalidSnappyDataException() {}
        InvalidSnappyDataException(String^ message) : SnappyException(message) { }
        InvalidSnappyDataException(String^ message, Exception^ inner) : SnappyException(message, inner) {}
    protected:
        InvalidSnappyDataException(SerializationInfo^ info, StreamingContext context) : SnappyException(info, context) {}
    };
}}