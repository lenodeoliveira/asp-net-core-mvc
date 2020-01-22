using System;


namespace SalesWebMvc.Services.Exception
{
    public class IntegrityException : ApplicationException
    {
        public IntegrityException(string message) : base(message)
        {

        }
    }
}
