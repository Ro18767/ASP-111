﻿using ASP_111.Data;
using ASP_111.Models.User;
using ASP_111.Services;
using ASP_111.Services.Hash;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Security.Claims;

namespace ASP_111.Controllers
{
    public class UserController : Controller
    {

        private readonly DataContext _dataContext;


        private readonly ILogger<UserController> _logger;
        private readonly ValidatorService _validatorService;
        private readonly IHashService _hashService;

        public UserController(DataContext dataContext, ILogger<UserController> logger, ValidatorService validatorService, IHashService hashService)
        {
            _dataContext = dataContext;
            _logger = logger;
            _validatorService = validatorService;
            _hashService = hashService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ViewResult Profile()
        {
            String? userId = HttpContext.User.Claims
                         .FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

            ProfileViewModel model = null!;
            if (userId is not null)
            {
                // находим данные о пользователе по ид
                var user = _dataContext.Users.Find(Guid.Parse(userId));
                if (user is not null)
                {
                    model = new()
                    {
                        Name = user.Name,
                        Email = user.Email ?? "",
                        Avatar = user.Avatar ?? "no-photo.png",
                        CreatedDt = user.CreatedDt,
                        Login = user.Login,
                    };
                }
            }
            return View(model);
        }



        [HttpPost]  // этот метод будет вызываться по AJAX при аутентификации
        public JsonResult Auth([FromBody] AuthAjaxModel model)
        {
            var user = _dataContext.Users.FirstOrDefault(
                u => u.Login == model.Login
                    && u.PasswordHash == _hashService.GetHash(model.Password)
                );
            // сохранение информации об аутентификации в сессии
            if (user != null)
            {
                HttpContext.Session.SetString("userId", user.Id.ToString());
            }
            return Json(new { Success = (user != null) });
        }

        public ViewResult SignUp(SignUpFormModel? formModel)
        {
            SignUpViewModel viewModel;
            if (Request.Method == "POST" && formModel != null)
            {
                viewModel = ValidateSignUpForm(formModel);
                viewModel.FormModel = formModel;
            }
            else
            {
                viewModel = new();
                viewModel.FormModel = null;
            }
            return View(viewModel);
        }

        public RedirectToActionResult Logout()
        {
            /* Выход из авторизированного режима всегда должен
             * перенаправить на страницу, которая доступна без авторизации
             * Чаще всего - на домашнюю страницу
             */
            HttpContext.Session.Remove("userId");
            return RedirectToAction("Index", "Home");
        }

        private SignUpViewModel ValidateSignUpForm(SignUpFormModel formModel)
        {
            SignUpViewModel viewModel = new();
            if (String.IsNullOrEmpty(formModel.Login))
            {
                viewModel.LoginMessage = "логин не может быть пустым";
            } else if (formModel.Login.Length < 3)
            {
                viewModel.LoginMessage = "логин слишком коротки (3 символа минимум)";
            }
            else if (_dataContext.Users.Any(u => u.Login == formModel.Login))
            {
                viewModel.LoginMessage = "даный логин уже занят";
            }
            else 
            {
                viewModel.LoginMessage = null;
            }

            if (String.IsNullOrEmpty(formModel.Password))
            {
                viewModel.PasswordMessage = "пароль не может быть пустым";
            }
            else if (formModel.Password.Length < 3)
            {
                viewModel.PasswordMessage = "пароль слишком коротки (3 символа минимум)";
            }
            else if (!Regex.IsMatch(formModel.Password, @"\d"))
            {
                viewModel.PasswordMessage = "пароль должен содержать цыфиры";
            }
            else
            {
                viewModel.PasswordMessage = null;
            }

            if (viewModel.PasswordMessage is not null)
            {
                viewModel.ReapetMessage = "пароль не может быть пустым";
            }
            else if (String.IsNullOrEmpty(formModel.RepeatPassword))
            {
                viewModel.ReapetMessage = "повторите пароль для потверждения";
            }
            else if (formModel.RepeatPassword != formModel.Password)
            {
                viewModel.ReapetMessage = "пароль не совпадает";
            }
            else
            {
                viewModel.ReapetMessage = null;
            }


           

            if (String.IsNullOrEmpty(formModel.RealName))
            {
                viewModel.RealNameMessage = null;
            }
            else
            {
                viewModel.RealNameMessage = null;
            }

            if (String.IsNullOrEmpty(formModel.Email))
            {
                viewModel.EmailMessage = null;
                formModel.Email = null;
            }
            else
            {

                try
                {
                    MailAddress m = new MailAddress(formModel.Email);
                    viewModel.EmailMessage = null;
                }
                catch (FormatException)
                {
                    viewModel.EmailMessage = "неправильный формат почты";
                }
            }
            if (!formModel.IsAgree)
            {
                viewModel.ConfirmMessage = "должен быть согласен на с условиями";
            }
            else
            {
                viewModel.ConfirmMessage = null;
            }
            String? nameAvatar = null;
            if (formModel.Avatar is not null)  // файл передан
            {
                /* При приема файла важно:
                 * - проверить допустимые расширения (тип)
                 * - проверить максимальный размер
                 * - заменить имя файла
                 * Создаем папку (вручную) wwwroot/avatars/
                 *  и в нее будем сохранять файлы, расширения - оставляем
                 *  какие есть, а вместо имени - используем GUID
                 */
                if (formModel.Avatar.Length > 1048576)
                {
                    viewModel.AvatarMessage = "Файл слишком большой (макс 1 МБ)";
                }
                // определяем расширение файла
                String ext = Path.GetExtension(formModel.Avatar.FileName);

                switch (ext)
                {
                    case ".png":
                    case ".jpg":
                        break;
                    default:
                        viewModel.AvatarMessage = "Файлы только .png и .jpg";
                        break;
                }
                // проверить расширение на перечень допустимых

                if(viewModel.AvatarMessage is null)
                {
                    // формируем имя для файла
                    nameAvatar = Guid.NewGuid().ToString() + ext;

                    formModel.Avatar.CopyTo(
                        new FileStream("wwwroot/avatars/" + nameAvatar, FileMode.Create));
                }
    
            }
            else
            {
                viewModel.AvatarMessage = null;
            }

            if (viewModel.LoginMessage == null
         && viewModel.PasswordMessage == null
         && viewModel.AvatarMessage == null)
            {
                // Все проверки пройдены успешно - добавляем пользователя в БД
                _dataContext.Users.Add(new()
                {
                    Id = Guid.NewGuid(),
                    Login = formModel.Login,
                    PasswordHash = _hashService.GetHash(formModel.Password),
                    Email = formModel.Email,
                    CreatedDt = DateTime.Now,
                    Name = formModel.RealName ?? formModel.Login,
                    Avatar = nameAvatar,
                });
                _dataContext.SaveChanges();
            }

            return viewModel;
        }
    }
}