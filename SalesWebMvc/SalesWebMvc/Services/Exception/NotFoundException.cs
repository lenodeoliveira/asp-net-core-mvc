using System;

namespace SalesWebMvc.Services.Exception
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string messege) : base(messege)
        {

        }
    }
}
