using System;
using Core.Infrastructure.Helper.ExceptionHelper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace Core.Api
{
    public partial class Startup
    {
		private void MiddlewareConfig(IApplicationBuilder app, ILoggerFactory loggerFactory)
		{
			app.UseMiddleware(typeof(ExceptionHandlingMiddleware));
		}
    }
}
