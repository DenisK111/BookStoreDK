using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreDK.KafkaCache.Contracts
{
    public interface ICacheItem<out T>
    {
        public DateTime LastUpdated { get; init; }
        T GetKey();

       
    }
}
