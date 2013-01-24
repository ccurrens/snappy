// Copyright (c) Christopher Currens.  Licensed under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0.html)

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Snappy.Net
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class InvalidSnappyDataException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public InvalidSnappyDataException()
        {
        }

        public InvalidSnappyDataException(string message) : base(message)
        {
        }

        public InvalidSnappyDataException(string message, Exception inner) : base(message, inner)
        {
        }

        protected InvalidSnappyDataException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}