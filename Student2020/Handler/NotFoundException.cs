using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Student2020.Handler
{
    public class NotFoundException: Exception
    {
        public NotFoundException(string msg) : base(msg)
        {

        }
    }
}
