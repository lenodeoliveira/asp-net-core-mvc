using System;

namespace SalesWebMvc.Services.Exception
{
    public class DbConcurrencyException : ApplicationException
    {
        public DbConcurrencyException(string messege) : base(messege)
        {

        }
    }
}
