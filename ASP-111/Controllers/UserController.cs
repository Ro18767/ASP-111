using ASP_111.Data;
using ASP_111.Models.User;
using ASP_111.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace ASP_111.Controllers
{
    public class UserController : Controller
    {

        private readonly DataContext _dataContext;

        private readonly ILogger<UserController> _logger;
        private readonly ValidatorService _validatorService;

        public UserController(DataContext dataContext, ILogger<UserController> logger, ValidatorService validatorService)
        {
            _dataContext = dataContext;
            _logger = logger;
            _validatorService = validatorService;
        }

        public IActionResult Index()
        {
            return View();
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

            if (formModel.Avatar is null)
            {
                viewModel.AvatarMessage = "аватара нет";
            }
            else
            {
                viewModel.AvatarMessage = null;
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

            return viewModel;
        }
    }
}