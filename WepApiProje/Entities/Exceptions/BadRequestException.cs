﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
	public abstract partial class BadRequestException : Exception
    {
        //abstract classlar newlenmez o yüzden protected yazıyoruz.
        protected BadRequestException(string message) : base(message)
        {

        }
    }
}
