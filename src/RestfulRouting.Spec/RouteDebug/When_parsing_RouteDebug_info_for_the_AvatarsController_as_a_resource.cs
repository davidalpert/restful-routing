using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Routing;
using Machine.Specifications;
using RestfulRouting.Mappers;
using RestfulRouting.RouteDebug;
using RestfulRouting.Spec.TestObjects;

namespace RestfulRouting.Spec.RouteDebug
{
    public class When_parsing_RouteDebug_info_for_the_AvatarsController_as_a_resource
    {
        private static RouteCollection _routes;
        private static RouteDebugViewModel _routeModel;

        private Establish context = () =>
            {
                _routes = new RouteCollection();

                new DebugRouteMapper("debug_route").RegisterRoutes(_routes);
                new ResourceMapper<AvatarsController>().RegisterRoutes(_routes);
            };

        private Because of = () => _routeModel = RouteDebugRouteParser.Parse(_routes);

        private It should_create_a_route_model = () => _routeModel.ShouldNotBeNull();

        private It should_set_the_debug_path = () => _routeModel.DebugPath.ShouldEqual("debug_route");

        private It should_include_all_the_endpoints = () => _routeModel.RouteInfos.Select(x => x.Endpoint)
                                                                       .ShouldContainOnly(new string[]
                                                                           {
                                                                               "routedebug#index",
                                                                               "avatars#Show",
                                                                               "avatars#Update",
                                                                               "avatars#New",
                                                                               "avatars#Edit",
                                                                               "avatars#Destroy",
                                                                               "avatars#Create"
                                                                           });

        private It should_include_all_the_details = () =>
            {
                var formattedRouteDetails = _routeModel.RouteInfos
                                                       .Select(info => string.Format("| {0} | {1} | {2} | {3} | {4} | {5} | {6} |",
                                                                                     info.Position, 
                                                                                     info.Endpoint, 
                                                                                     info.HttpMethod,
                                                                                     info.Name, 
                                                                                     info.Area, 
                                                                                     info.Path, 
                                                                                     info.Namespaces));

                var expectedRouteDetails = new[]
                    {

                        // # | endpoint         | method | name        | area | path        | namespace                       |
                        // ----------------------------------------------------------------------------------------------------
                        "| 1 | routedebug#index | GET    |             |      | debug_route | RestfulRouting.RouteDebug       |",
                        "| 2 | avatars#Show     | GET    | avatar      |      | avatar      | RestfulRouting.Spec.TestObjects |",
                        "| 3 | avatars#Update   | PUT    |             |      | avatar      | RestfulRouting.Spec.TestObjects |",
                        "| 4 | avatars#New      | GET    | new_avatar  |      | avatar/new  | RestfulRouting.Spec.TestObjects |",
                        "| 5 | avatars#Edit     | GET    | edit_avatar |      | avatar/edit | RestfulRouting.Spec.TestObjects |",
                        "| 6 | avatars#Destroy  | DELETE |             |      | avatar      | RestfulRouting.Spec.TestObjects |",
                        "| 7 | avatars#Create   | POST   |             |      | avatar      | RestfulRouting.Spec.TestObjects |"
                    }
                    .Select(s => s.Replace(@"\s+", RegexOptions.Multiline, " "))
                    .Select(s => s.Replace(@"\| (?=\|)", RegexOptions.Multiline, "|  "));

                formattedRouteDetails.ShouldContainOnly(expectedRouteDetails);
            };
    }
    
    public static partial class StringExtensions
    {
        public static string Replace(this string input, string regex, RegexOptions options, string replacement)
        {
            return new Regex(regex, options).Replace(input, replacement);
        }
    }
}