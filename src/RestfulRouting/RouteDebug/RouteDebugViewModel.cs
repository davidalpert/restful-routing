using System.Linq;
using System.Collections.Generic;

namespace RestfulRouting.RouteDebug
{
    public class RouteDebugViewModel
    {
        public string DebugPath { get; set; }
        public IList<DebugRouteInfo> RouteInfos { get; set; }
    }
}