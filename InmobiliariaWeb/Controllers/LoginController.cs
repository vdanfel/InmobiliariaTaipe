using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models;
using InmobiliariaWeb.Result;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaWeb.Controllers
{
    public class LoginController:Controller
    {
        private readonly ILoginService _loginService;
        public LoginController(ILoginService loginService) 
        { 
            _loginService = loginService;
        }
        public IActionResult Index(string mensaje)
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            loginViewModel.Mensaje = mensaje;
            return View(loginViewModel);
        }
        public async Task<IActionResult> ValidarSesion(LoginViewModel loginViewModel)
        {
            LoginResult loginResult = await _loginService.ValidarLogin(loginViewModel.Usuario,loginViewModel.Clave);

            if (!string.IsNullOrEmpty(loginResult.Mensaje))
            {
                HttpContext.Session.SetInt32("IdentUsuario", loginResult.IdentUsuario);
                HttpContext.Session.SetString("Usuario", loginResult.Usuario);
                HttpContext.Session.SetInt32("Ident005TipoUsuario", loginResult.Ident005TipoUsuario);
                HttpContext.Session.SetString("NombreCompleto", loginResult.NombreCompleto);
                return RedirectToAction("Index", "Dashboard");
            }
            return RedirectToAction("Index","Login", new { mensaje = "Usuario o Clave Incorrecto" });
        }
    }
}
