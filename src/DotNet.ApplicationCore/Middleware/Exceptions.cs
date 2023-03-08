using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.ApplicationCore.Middleware
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        { 
        }
    }
    public class KeyNotFoundException : Exception
    {
        public KeyNotFoundException(string message) : base(message)
        { }
    }
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        { }
    }
    public class NotImplementedException : Exception
    {
        public NotImplementedException(string message) : base(message)
        { }
    }
    public class UnauthorizedAccessException : Exception
    {
        public UnauthorizedAccessException(string message) : base(message)
        { }
    }
    public class HadReferanceException : Exception
    {
        public HadReferanceException(string message) : base(message)
        { }
    }
}
