using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using System;
using System.Linq;

namespace SPX_WEBAPI.Utils
{
    public static class FilterContextExtensionMethods
    {
        public static bool ContainsRequestMethods(this FilterContext context, params HttpMethod[] methods)
        {
            if (methods.Any(method => context.HttpContext.Request.Method.Equals(method.ToString(), StringComparison.InvariantCultureIgnoreCase)))
                return true;

            return false;
        }
    }
}
