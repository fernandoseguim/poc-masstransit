using System;

namespace Orquestration.Saga.Common
{
    public class BrokerSettings
    {
        public string Host { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string InputQueue { get; set; }
    }
}
