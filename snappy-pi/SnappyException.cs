using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Snappy.Net
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class SnappyException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public SnappyException()
        {
        }

        public SnappyException(string message) : base(message)
        {
        }

        public SnappyException(string message, Exception inner) : base(message, inner)
        {
        }

        protected SnappyException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
