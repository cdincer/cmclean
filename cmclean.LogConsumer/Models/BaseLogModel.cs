using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmclean.LogConsumer.Models
{
    public class BaseLogModel
    {

        public BaseLogModel()
        {

        }
        public BaseLogModel(string message)
        {
            Message = message;
        }
        public string? RequestID { get; set; }
        public DateTime RequestTime { get; set; }
        public DateTime ResponseTime { get; set; }
        public double Duration { get; set; }
        public string? Message { get; set; }
    }
}
