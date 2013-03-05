using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Routing;

namespace RestfulRouting.RouteDebug
{
    public class RouteDebugRouteParser
    {
        public static RouteDebugViewModel Parse(RouteCollection routes)
        {
            var model = new RouteDebugViewModel
                {
                    RouteInfos = new List<DebugRouteInfo>()
                };
            int position = 1;
            foreach (var routeBase in routes)
            {
                var route = routeBase as Route;
                if (route != null)
                {
                    // issue: #33 Fix
                    var httpMethodConstraint =
                        (route.Constraints ?? new RouteValueDictionary())["httpMethod"] as HttpMethodConstraint;

                    ICollection<string> allowedMethods = new string[] {};
                    if (httpMethodConstraint != null)
                    {
                        allowedMethods = httpMethodConstraint.AllowedMethods;
                    }

                    var namespaces = new string[] {};
                    if (route.DataTokens != null && route.DataTokens["namespaces"] != null)
                        namespaces = (route.DataTokens["namespaces"] ?? new string[0]) as string[];
                    var defaults = new RouteValueDictionary();
                    if (route.Defaults != null)
                        defaults = route.Defaults;
                    if (route.DataTokens == null)
                        route.DataTokens = new RouteValueDictionary();

                    var namedRoute = route as NamedRoute;
                    var routeName = "";
                    if (namedRoute != null)
                    {
                        routeName = namedRoute.Name;
                    }

                    model.RouteInfos.Add(new DebugRouteInfo
                        {
                            Position = position,
                            HttpMethod = string.Join(", ", allowedMethods.ToArray()),
                            Path = route.Url,
                            Endpoint = defaults["controller"] + "#" + defaults["action"],
                            Area = route.DataTokens["area"] as string,
                            Namespaces = string.Join(" ", (namespaces).ToArray()),
                            Name = routeName
                        });
                }
                else
                {
                    const string unknown = "???";
                    var type = routeBase.GetType();
                    model.RouteInfos.Add(new DebugRouteInfo
                        {
                            Position = position,
                            HttpMethod = "*",
                            Path = type.FullName,
                            Endpoint = unknown,
                            Area = unknown,
                            Namespaces = type.Namespace,
                            Name = type.Name + " (external)",
                            IsUnknown = true
                        });
                }

                position++;
            }

            var debugPath = (from p in model.RouteInfos
                             where
                                 p.Endpoint.Equals("routedebug#index", StringComparison.InvariantCultureIgnoreCase)
                             select p.Path.Replace("{name}", string.Empty)).FirstOrDefault();
            model.DebugPath = debugPath;
            return model;
        }
    }
}