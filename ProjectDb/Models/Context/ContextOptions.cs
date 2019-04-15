using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDb.Models.Context
{
    public class ContextOptions
    {
        /// <summary>
        /// fetch connectionstring
        /// </summary>
        public ContextOptions()
        {
            ConnectionString = Startup.ConnectionString;
        }
        public string ConnectionString { get; set; }
    }
}
