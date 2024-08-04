using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.ActionFilters;

/// <summary>
///  Bazi actionlarda if(!ModelState.IsValid) return UnprocessibleEntity(ModelState);
/// kod parcacigini tekrar tekrar yazmamk adina bu aciton attribute'u olusturdum
/// kullanmak icin Controller icindedeki Actionda attribute olarak kullanılabilir
/// Ör:
///
/// [ServiceFilter(typeof(IsModelStateNotValid))]
/// async Task<IActionResult> UpdateBookAsync() { ..
/// 
/// </summary>
public class IsModelStateNotValid : ActionFilterAttribute
{
    // Action calismadan once calissin
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var controller = context.RouteData.Values["controller"]; // hangi controller calisiyor? or: BooksController
        var action = context.RouteData.Values["action"]; // Hangi action calisiyor or: GetAllBooksAsync

        // bu action filtera ihtiyac duydugum tum actionlarda ismi Dto olan bir parametre var (BookDto, BookDtoForUpdate ...)
        var param = context.ActionArguments.SingleOrDefault(p => p.Value.ToString().Contains("Dto")).Value;

        if (param is null)
        {
            context.Result = new BadRequestObjectResult($"Dto parameter is null.\nController: {controller}\nAction:{action}");
            return; // 400
        }

        if (!context.ModelState.IsValid)
        {
            context.Result = new UnprocessableEntityObjectResult(context.ModelState); // 422
        }
    }
}