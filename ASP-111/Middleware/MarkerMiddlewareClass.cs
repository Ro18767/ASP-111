namespace ASP_111.Middleware
{
    public class MarkerMiddleware
    {
        private readonly RequestDelegate _next;

        public MarkerMiddleware(RequestDelegate next)
        {
            _next = next;
            /* Похожее на инъекцию внедрение, но это не служба, а "связка"
             * цепочки Middleware путем передачи в каждый из классов ссылки
             * на следующий элемент цепочки.
             */
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Items.Add("marker", "TheMarker");
            await _next(context);
        }

        /* Классы Middleware не наследуются от общего родителя, однако
         * обязаны содержать метод 
         *   public async Task InvokeAsync(HttpContext context)
         * либо
         *   public void Invoke(HttpContext context) { }
         * В методе должен быть вызов
         *    await _next(context);   /   _next(context);
         * Все, что выполняется ДО этого вызова - "прямой ход",
         *   ПОСЛЕ - "обратный ход"
         */
    }

    // класс-расширение
    public static class MarkerMiddlewareExtension
    {
        /* Метод-расширение: во всех объектах типа IApplicationBuilder
         * будет доступен метод UseMarker
         * (в Program.cs:   app.UseMarker()  )
         */
        public static IApplicationBuilder UseMarker(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MarkerMiddleware>();
        }
    }
}