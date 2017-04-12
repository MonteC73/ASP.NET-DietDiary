using CountingKs.Services;
using Newtonsoft.Json.Serialization;
using System.Web.Http;
using System.Web.Http.Dispatcher;

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

        config.Routes.MapHttpRoute(
            name: "Token",
            routeTemplate: "api/token",
            defaults: new {controller = "diarysummary"}
            );



        config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;
        config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling =
            Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        config.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);

        //var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().FirstOrDefault();
        //jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

        // Add support for JSONP
        ////var formatter = new JsonpMediaTypeFormatter(jsonFormatter); // in case callback name is taken, set new in contructor.
        ////config.Formatters.Insert(0, formatter);

        // Replace the Controller Configuration to support Versioned Routing
        config.Services.Replace(typeof(IHttpControllerSelector), new CountingKsControllerSelector(config));

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