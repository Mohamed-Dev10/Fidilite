using System;
using System.Collections.Generic;
using System.Text;

namespace BRICOMA.ECOMMERCE.Models.Helpers
{
    public class BaseCustomException : Exception
    {
        public BaseCustomException(string message) : base(message)
        {
        }

        public BaseCustomException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class CustomBadRequestException : BaseCustomException
    {
        public CustomBadRequestException(string message) : base(message) { }
        public CustomBadRequestException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class CustomNotFoundException : BaseCustomException
    {
        public CustomNotFoundException(string message) : base(message) { }
        public CustomNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class CustomUnauthorizedException : BaseCustomException
    {
        public CustomUnauthorizedException(string message) : base(message) { }
        public CustomUnauthorizedException(string message, Exception innerException) : base(message, innerException) { }
    }
}

