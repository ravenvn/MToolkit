using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MToolkit
{
    class ViewData
    {
        public string TitleVideoIds { get; set; }
        public Account[] Accounts { get; set; }
        public bool Sub { get; set; }
        public bool Like { get; set; }
        public int SubRatio { get; set; }
        public int LikeRatio { get; set; }
        public string FilterType { get; set; }
        public int DurationMin { get; set; }
        public int DurationMax { get; set; }

    }
}
