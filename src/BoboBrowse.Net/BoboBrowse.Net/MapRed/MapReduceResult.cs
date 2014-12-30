﻿// Version compatibility level: 3.1.0
namespace BoboBrowse.Net.MapRed
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [Serializable]
    public class MapReduceResult
    {
        public MapReduceResult()
        {
            MapResults = new ArrayList(200);
        }

        public virtual ArrayList MapResults { get; set; }
        public virtual MapReduceResult ReduceResult { get; set; }
    }
}
