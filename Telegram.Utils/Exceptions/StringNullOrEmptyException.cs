using System;

namespace Telegram.Utils.Exceptions
{
    public class StringNullOrEmptyException: Exception
    {
        public StringNullOrEmptyException()
            : base()
        {
        }

        public StringNullOrEmptyException(string message)
            : base(message)
        {
        }

        public StringNullOrEmptyException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}