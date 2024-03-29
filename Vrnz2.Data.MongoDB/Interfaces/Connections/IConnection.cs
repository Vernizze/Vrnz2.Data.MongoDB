﻿using System;
using Vrnz2.Data.MongoDB.Connections;

namespace Vrnz2.Data.MongoDB.Interfaces.Connections
{
    public interface IConnection
        : IDisposable
    {
        MongoClientHelper GetClient();
        void StopClient();
    }
}
