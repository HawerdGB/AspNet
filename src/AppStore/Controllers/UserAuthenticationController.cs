using AppStore.Models.DTO;
using AppStore.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AppStore.Controllers
{
    public class UserAuthenticationController :Controller
    {
        private readonly IUserAuthenticationService _authService;

        public UserAuthenticationController(IUserAuthenticationService authService)
        {
            _authService =  authService;
        }

        public IActionResult Login()
        {
            return View();
        }




        [HttpPost]
        public async Task<IActionResult> Login(LoginModel login)
        {
            if(!ModelState.IsValid)
            {
               return View(login);
            }
            var resultado =await _authService.LoginAsync(login);

            if(resultado.StatusCode == 1)
            {
                return RedirectToAction("Index", "Home");
            }else 
            {
                TempData["msg"] = resultado.Message;
                return RedirectToAction(nameof(Login));
            }

        }

        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}