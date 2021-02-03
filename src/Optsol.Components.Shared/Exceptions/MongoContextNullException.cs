﻿using System;

namespace Optsol.Components.Shared.Exceptions
{
    public class MongoContextNullException : Exception
    {
        public MongoContextNullException()
            : base("O parametro MongoContext não foi resolvido pela injeção de dependência")
        {

        }
    }
}
