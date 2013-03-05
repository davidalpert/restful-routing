using System.Linq;
using System.Collections.Generic;
using System.Web.Routing;

namespace RestfulRouting.RouteDebug
{
    /// <summary>
    /// Summarizes the details of a <see cref="Route"/>.
    /// </summary>
    public class DebugRouteInfo
    {
        public int Position { get; set; }
        public string HttpMethod { get; set; }
        public string Path { get; set; }
        public string Endpoint { get; set; }
        public string Area { get; set; }
        public string Namespaces { get; set; }
        public string Name { get; set; }
        public bool IsUnknown { get; set; }
    }
}