using ASP_111.Data;
using System.Security.Claims;

namespace ASP_111.Middleware
{
    public class AuthSessionMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthSessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, DataContext dataContext)
        {
            if (context.Session.Keys.Contains("userId"))  // есть сохраненные данные
            {
                var user = dataContext.Users.Find(Guid.Parse(
                    context.Session.GetString("userId")!
                ));
                if (user != null)
                {
                    // Авторизация в ASP построена на Claims-свойствах
                    var claims = new Claim[] {
                        new Claim(ClaimTypes.Sid, user.Id.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, user.Login),
                        new Claim(ClaimTypes.Email, user.Email ?? ""),
                        new Claim(ClaimTypes.UserData, user.Avatar ?? "")
                    };
                    context.User = new ClaimsPrincipal(
                        new ClaimsIdentity(claims, nameof(AuthSessionMiddleware))
                    );
                }
            }
            /* Д.З. При успешной аутентификации производить перезагрузку
             * (обновление) страницы, иначе вывести сообщение об отказе.
             * Ограничить размеры изображения-аватарки в header
             * Добавить кнопку "Выйти" (подобрать картинку), **реализовать ее работу
             */
            await _next(context);
        }
    }

    public static class AuthSessionMiddlewareExtension
    {
        public static IApplicationBuilder
            UseAuthSession(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AuthSessionMiddleware>();
        }
    }
}