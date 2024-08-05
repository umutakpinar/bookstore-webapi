using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;

namespace Presentation.ActionFilters;

public class ValidateMediaType : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        // gelen requestte accept ifadesi var mi?
        var acceptHeaderPreset = context.HttpContext
            .Request
            .Headers
            .ContainsKey("Accept");

        if (!acceptHeaderPreset)
        {
            context.Result = new BadRequestObjectResult("Accept header is missing!");
        }
        
        var mediaType = context.HttpContext
            .Request
            .Headers["Accept"]
            .FirstOrDefault();

        if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue? outMediaType))
        {
            context.Result = new
                BadRequestObjectResult("Media type not present\nPlease add Accept header with required media type.");
        }
        
        context.HttpContext.Items.Add("AcceptHeaderMediaType", outMediaType);
    }
}