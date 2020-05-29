using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class CommandRequest
    {
        public int PointId { get; set; }
        public int Address { get; set; }
        public int Value { get; set; }
    }
}
