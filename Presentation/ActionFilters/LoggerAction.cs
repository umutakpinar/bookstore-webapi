using Entities.LogModel;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using NLog.Fluent;
using Services.Contracts;

namespace Presentation.ActionFilters;

public class LoggerAction : ActionFilterAttribute
{
    private readonly ILoggerService _logger;

    public LoggerAction(ILoggerService logger)
    {
        _logger = logger;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInfo(Log(("OnActionExecuting"), context.RouteData));
    }

    private string Log(string modelName, RouteData routeData)
    {
        var logDetails = new LogDetails()
        {
            ModelName = modelName,
            Controller = routeData.Values["controller"],
            Action =  routeData.Values["action"],
            Id = routeData.Values.Count >= 3 ? routeData.Values["Id"] : null 
        };

        return logDetails.ToString();
    }
}