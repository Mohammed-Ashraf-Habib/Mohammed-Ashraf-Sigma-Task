using Microsoft.AspNetCore.Builder;

namespace Task.WepApi.Middlewares
{
    public static class RequestMiddlewareExtensions
    {
        public static void ConfigureCustomRequestMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestMiddleware>();
        }
    }
}
