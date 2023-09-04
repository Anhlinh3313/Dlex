using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Core.Infrastructure.Helper.ExceptionHelper
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _delegate;

		public ExceptionHandlingMiddleware(RequestDelegate next)
		{
			_delegate = next;
		}
		public async Task Invoke(HttpContext httpContext)
		{
			try
			{
				await _delegate(httpContext);
			}
			catch (Exception ex)
			{
				await ResponseExceptionHelper.HandleExceptionAsync(httpContext, ex);
			}
		}
	}
}
