﻿
using System;

namespace FixJson
{
    [Serializable]
    public class Management
    {
        public int mgmtID { get; set; }
        public string name { get; set; }
        public string market { get; set; }
        public string state { get; set; }
    }

}
