using Microsoft.AspNetCore.Mvc;

namespace ASP_111.Models.User
{
    public class SignUpViewModel
    {
        // если null, то проверка прошла упешно , а если есть значение, то это соообщение об ошибке
        public string? LoginMessage { get; set; }

        public string? PasswordMessage { get; set; }
        public string? ReapetMessage { get; set; }
        public string? RealNameMessage { get; set; }
        public string? EmailMessage { get; set; }
        public string? AvatarMessage { get; set; }
        public string? ConfirmMessage { get; set; }
        public SignUpFormModel? FormModel { get; set; }
    }
}
