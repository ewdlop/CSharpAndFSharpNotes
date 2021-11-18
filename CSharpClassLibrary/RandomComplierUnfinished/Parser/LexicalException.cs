using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CSharpClassLibrary.RandomComplier
{
    [Serializable]
    public class LexicalException : Exception
    {
        public LexicalException() : base("Unexpected character") {}
        public LexicalException(string message) : base(string.Format("Unexpected character {0}", message)) {}

        public LexicalException(string message, Exception innerException) : base(string.Format("Unexpected character {0}", message), innerException) {}

        protected LexicalException(SerializationInfo info, StreamingContext context) : base(info, context) {}

    }
}
