using System.Web.Http;
using CountingKs.Filters;

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


#if !DEBUG
        // Force HTTPS on entire API
        config.Filters.Add(new RequireHttpsAttribute());
#endif
        
      // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
      // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
      // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
      //config.EnableQuerySupport();

        ////var jsonFormatter = config.Formatters.OfType<System.Net.Http.Formatting.JsonMediaTypeFormatter>().FirstOrDefault();
        ////jsonFormatter.SerializeSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    }
  }
}