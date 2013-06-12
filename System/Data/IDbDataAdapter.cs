﻿namespace System.Data
{
    using System;

    public interface IDbDataAdapter : IDataAdapter
    {
        IDbCommand DeleteCommand { get; set; }

        IDbCommand InsertCommand { get; set; }

        IDbCommand SelectCommand { get; set; }

        IDbCommand UpdateCommand { get; set; }
    }
}

