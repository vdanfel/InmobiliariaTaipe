using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models;
using InmobiliariaWeb.Models.Programa;
using InmobiliariaWeb.Result;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaWeb.Controllers
{
    public class ProgramaController:Controller
    {
        private readonly IProgramaService _programaService;

        public ProgramaController(IProgramaService programaService)
        {
            _programaService = programaService;
        }
        public async Task<IActionResult> Index(ProgramaViewModel programaViewModel) 
        {
            programaViewModel.ProgramaList = await _programaService.BandejaPrograma(programaViewModel.Buscar);

            return View(programaViewModel);
        }
        [HttpGet]
        public IActionResult Crear()
        {
            ProgramaCrearViewModel programaCrearViewModel = new ProgramaCrearViewModel();
            return View(programaCrearViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Crear(ProgramaCrearViewModel programaCrearViewModel)
        {
            if (programaCrearViewModel.ViewPrograma.AreaTotal < programaCrearViewModel.ViewPrograma.AreaLotizada)
            {
                programaCrearViewModel.Mensaje = "El Área Total no puede ser menor que el Área Lotizada";
                return View(programaCrearViewModel);
            }
            else
            {
                LoginResult loginResult = new LoginResult();
                loginResult = DatosUsuarioLogin();
                ViewPrograma viewprograma = new ViewPrograma();
                viewprograma.IdentPrograma = await _programaService.RegistrarPrograma(programaCrearViewModel.ViewPrograma, loginResult);
                if (viewprograma.IdentPrograma > 0)
                {
                    var mensaje = await _programaService.RegistrarManzanas(programaCrearViewModel.ViewPrograma, loginResult);
                    if (mensaje == "ok")
                    {
                        viewprograma.Mensaje = "Se registró con éxito";
                    }
                    else
                    {
                        viewprograma.Mensaje = "Se registró el Programa pero no se registraron los lotes";
                    }
                    return RedirectToAction("Actualizar", "Programa", new { IdentPrograma = viewprograma.IdentPrograma });
                }
                else
                {
                    programaCrearViewModel.ViewPrograma.Mensaje = "Problemas al intentar registrar Programa";
                    return View(programaCrearViewModel.ViewPrograma);
                }
            }
            
            
        }
        public async Task<IActionResult> Actualizar(int IdentPrograma) 
        {
            var viewPrograma = await _programaService.BuscarProgramaIdentPrograma(IdentPrograma);
            return View(viewPrograma);
        }
        public LoginResult DatosUsuarioLogin()
        {
            LoginResult loginResult = new LoginResult();
            loginResult.IdentUsuario = (int)HttpContext.Session.GetInt32("IdentUsuario");
            loginResult.Usuario = HttpContext.Session.GetString("Usuario");
            loginResult.Ident005TipoUsuario = (int)HttpContext.Session.GetInt32("Ident005TipoUsuario");
            loginResult.NombreCompleto = HttpContext.Session.GetString("NombreCompleto");
            return loginResult;
        }
    }
}
