﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Student2020.Handler
{
    public class BadRequestException: Exception
    {
        public BadRequestException(string msg) : base(msg)
        {

        }
    }
}
