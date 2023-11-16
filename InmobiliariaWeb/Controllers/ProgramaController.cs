using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models;
using InmobiliariaWeb.Models.Programa;
using InmobiliariaWeb.Result;
using InmobiliariaWeb.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaWeb.Controllers
{
    public class ProgramaController:Controller
    {
        private readonly IProgramaService _programaService;
        private readonly IPersonaService _personaService;
        private readonly ITablasService _tablasService;

        public ProgramaController(IProgramaService programaService, IPersonaService personaService, ITablasService tablasService)
        {
            _programaService = programaService;
            _personaService = personaService;
            _tablasService = tablasService;
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
            viewPrograma.TipoPropietario = await _tablasService.ListarTipoPropietario();
            viewPrograma.viewPropietarios = await _programaService.ListarPropietario(IdentPrograma);
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
        [HttpGet]
        public async Task<IActionResult> BuscarPersonas(string buscar)
        {
            try
            {
                var personas = await _personaService.PersonaBandeja(buscar);
                return Json(personas);
            }
            catch (Exception ex)
            {
                // Manejar errores según sea necesario
                return Json(new { error = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> RegistrarPropietario(int identPrograma, int identPersona, int tipoPropietario, string numeroPartida)
        {
            try
            {
                LoginResult loginResult = DatosUsuarioLogin();

                // Llamar al método del servicio para registrar el propietario
                int identProgramaPropietario = await _programaService.RegistrarPropietario(identPrograma, identPersona, tipoPropietario, numeroPartida, loginResult.IdentUsuario);
                if (identProgramaPropietario > 0)
                {
                    // Obtener el propietario recién registrado
                    var propietario = await _programaService.ListarPropietario(identPrograma);

                    // Devolver el propietario como resultado
                    return Json(propietario);
                }
                else
                {
                    return Json(new { error = "No se pudo cargar información del proveedor" });
                }
            }
            catch (Exception ex)
            {
                // Manejar errores según sea necesario
                return Json(new { error = ex.Message });
            }
        }
    }
}
