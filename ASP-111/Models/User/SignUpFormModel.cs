using Microsoft.AspNetCore.Mvc;

namespace ASP_111.Models.User
{
    public class SignUpFormModel
    {
        [FromForm(Name = "user-login")]  // <input ... name="user-login"
        public string Login { get; set; } = null!;

        [FromForm(Name = "user-password")]
        public string Password { get; set; } = null!;

        [FromForm(Name = "user-repeat")]
        public string RepeatPassword { get; set; } = null!;

        [FromForm(Name = "user-name")]
        public string? RealName { get; set; } = null!;

        [FromForm(Name = "user-email")]
        public string? Email { get; set; } = null!;

        [FromForm(Name = "user-avatar")]
        public IFormFile Avatar { get; set; } = null!;  // <input type="file" name="user-avatar"

        [FromForm(Name = "user-confirm")]
        public bool IsAgree { get; set; }   // <input type = "checkbox"

    }
}

/* Модели и передача данных
 * В ASP модели - классы, использующиеся для передачи данных.
 * Очень часто каждое представление имеет свою модель, поэтому моделей
 * много и их следует группировать по папкам с именами контроллеров.
 * 
 * Для приема данных от форм также создают модели. При этом дополнительная
 * задача таких моделей - согласовать имена полей в форме (разметке)
 * и в модели
 */