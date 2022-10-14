using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreDK.KafkaCache.Contracts
{
    public interface IKafkaConsumerSettings
    {
        public string BootstrapServers { get; set; }      

    }
}
