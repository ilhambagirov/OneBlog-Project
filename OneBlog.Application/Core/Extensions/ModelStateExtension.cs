using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Portfolio.Application.Core.Extension
{
    static public partial class Extension
    {
        static public bool IsModelStateValid(this IActionContextAccessor ctx)
        {
            return ctx.ActionContext.ModelState.IsValid;
        }
    }
}
