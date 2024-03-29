﻿using System.Collections.Concurrent;
using System.Collections.Generic;

namespace QuickNet
{
    internal class ClientPeer
    {
        public Dictionary<string, object> outboundCache = new Dictionary<string, object>();
        public readonly ConcurrentQueue<(string key, object data)> reliableOutboundQueue = new ConcurrentQueue<(string key, object data)>();
    }
}
