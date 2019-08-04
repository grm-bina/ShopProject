using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web
{
    public sealed class DelatedSessionData
    {
        private static DelatedSessionData _instance = null;
        private static readonly object _lock = new object();
        private DelatedSessionData()
        {
            ShoppingCart =  new ConcurrentQueue<int>();
        }
        public static DelatedSessionData GetInstance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new DelatedSessionData();
                    return _instance;
                }
            }
        }
        
        public static ConcurrentQueue<int> ShoppingCart { get; private set; } 
    }
}