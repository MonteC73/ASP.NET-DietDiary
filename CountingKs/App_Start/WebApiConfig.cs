using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using CountingKs.Filters;
using CountingKs.Services;
using Newtonsoft.Json.Serialization;
using WebApiContrib.Formatting.Jsonp;

namespace CountingKs
{
  public static class WebApiConfig
  {
    public static void Register(HttpConfiguration config)
    {
      //config.Routes.MapHttpRoute(
      //    name: "DefaultApi",
      //    routeTemplate: "api/{controller}/{id}",
      //    defaults: new { id = RouteParameter.Optional }
      //);

        config.Routes.MapHttpRoute(
            name: "Food",
            routeTemplate: "api/nutrition/foods/{foodid}",
            defaults: new {controller = "foods", foodid = RouteParameter.Optional}
            );

        config.Routes.MapHttpRoute(
            name: "Measures",
            routeTemplate: "api/nutrition/foods/{foodid}/measures/{id}",
            defaults: new {controller = "measures", diaryid = RouteParameter.Optional}
            );

        config.Routes.MapHttpRoute(
            name: "Diaries",
            routeTemplate: "api/user/diaries/{diaryid}",
            defaults: new { controller = "diaries", diaryid = RouteParameter.Optional }
            );


        config.Routes.MapHttpRoute(
            name: "DiaryEntries",
            routeTemplate: "api/user/diaries/{diaryid}/entries/{id}",
            defaults: new { controller = "diaryentries", id = RouteParameter.Optional }
        );

        config.Routes.MapHttpRoute(
            name: "DiarySummary",
            routeTemplate: "api/user/diaries/{diaryid}/summary",
            defaults: new { controller = "diarysummary" }
            );



        var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().FirstOrDefault();
        jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

        // Add support for JSONP
        ////var formatter = new JsonpMediaTypeFormatter(jsonFormatter); // in case callback name is taken, set new in contructor.
        ////config.Formatters.Insert(0, formatter);


#if !DEBUG
        // Force HTTPS on entire API
        config.Filters.Add(new RequireHttpsAttribute());
#endif
        
      // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
      // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
      // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
      //config.EnableQuerySupport();
    }
  }
}