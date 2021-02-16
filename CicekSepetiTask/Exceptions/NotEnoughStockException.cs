using System;

namespace CicekSepetiTask.Exceptions
{
    public class NotEnoughStockException : Exception
    {
        public NotEnoughStockException()
        {
        }

        public NotEnoughStockException(string message)
            : base(message)
        {
        }

        public NotEnoughStockException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}