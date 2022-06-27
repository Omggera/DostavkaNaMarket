using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;

namespace DostavkaNaMarket.Models
{
    public class LoginModel: ComponentBase
    {
        public LoginModel()
        {
            LoginData = new LoginViewModel();
        }

        public LoginViewModel LoginData { get; set; }

        protected Task LoginAsync()
        {
            return Task.CompletedTask;
        }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string AdminName { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string AdminPassword { get; set; }
    }
}
