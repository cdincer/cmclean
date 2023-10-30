using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmclean.LogConsumer.Models
{
    public class ElasticSearchConfigModel
    {
        public string ConnectionString { get; set; } = null!;
        public int PingTimeMilliSeconds { get; set; }
    }
}
