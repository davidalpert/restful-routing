﻿using System.Web.Routing;
using RestfulRouting.Mappers;

namespace RestfulRouting
{
    public abstract class RouteSet
    {
        public static bool LowercaseUrls = true;
        public static bool LowercaseDefaults = true;
        public static bool LowercaseActions = true;
        public static bool MapDelete = false;

        public abstract void Map(IMapper map);

        public void RegisterRoutes(RouteCollection routes, string[] namespaces = null)
        {
            var mapper = new Mapper(namespaces);
            Map(mapper);

            mapper.RegisterRoutes(routes);
        }
    }
}
