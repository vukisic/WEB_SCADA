using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    /// <summary>
    /// Represents command model from client
    /// </summary>
    public class CommandRequest
    {
        /// <summary>
        /// Id of Point
        /// </summary>
        public int PointId { get; set; }
        /// <summary>
        /// Address of point
        /// </summary>
        public int Address { get; set; }
        /// <summary>
        /// New value (commanded value)
        /// </summary>
        public int Value { get; set; }
    }
}
