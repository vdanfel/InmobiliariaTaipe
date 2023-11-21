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
        public async Task<IActionResult> Crear()
        {
            ProgramaCrearViewModel programaCrearViewModel = new ProgramaCrearViewModel();
            programaCrearViewModel.Manzanas = await _tablasService.ListarManzanas();
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
                programaCrearViewModel.ViewPrograma.IdentPrograma = await _programaService.RegistrarPrograma(programaCrearViewModel.ViewPrograma, loginResult);
                ViewPrograma viewprograma = new ViewPrograma();
                viewprograma = programaCrearViewModel.ViewPrograma;
                if (viewprograma.IdentPrograma > 0)
                {
                    var mensaje = await _programaService.RegistrarManzanas(viewprograma, loginResult);
                    if (mensaje == "ok")
                    {
                        viewprograma.Mensaje = "Se registró con éxito";
                    }
                    else
                    {
                        viewprograma.Mensaje = "Se registró el Programa pero no se registraron los lotes";
                    }
                    return RedirectToAction("Actualizar", "Programa", new { IdentPrograma = viewprograma.IdentPrograma, Mensaje=viewprograma.Mensaje });
                }
                else
                {
                    programaCrearViewModel.ViewPrograma.Mensaje = "Problemas al intentar registrar Programa";
                    return View(programaCrearViewModel.ViewPrograma);
                }
            }
        }
        [HttpGet]
        public async Task<IActionResult> Actualizar(int IdentPrograma, string Mensaje) 
        {
            var viewPrograma = await _programaService.BuscarProgramaIdentPrograma(IdentPrograma);
            if (Mensaje != null)
            {
                viewPrograma.Mensaje = Mensaje;
            }
            viewPrograma.TipoPropietario = await _tablasService.ListarTipoPropietario();
            viewPrograma.viewPropietarios = await _programaService.ListarPropietario(IdentPrograma);
            viewPrograma.viewManzana = await _programaService.ListarManzanasPrograma(IdentPrograma);
            viewPrograma.manzanas = await _tablasService.ListarManzanas();
            return View(viewPrograma);
        }
        [HttpPost]
        public async Task<IActionResult> ActualizarPrograma(ViewPrograma viewPrograma)
        {
            LoginResult loginResult = new LoginResult();
            loginResult = DatosUsuarioLogin();
            if (viewPrograma.Confirmacion == "OK")
            {
                var mensaje = await _programaService.AnularManzanasList(viewPrograma.IdentPrograma,loginResult.IdentUsuario);
                mensaje = await _programaService.RegistrarManzanas(viewPrograma, loginResult);
            }

            viewPrograma.Mensaje = await _programaService.ActualizarPrograma(viewPrograma, loginResult);
            return RedirectToAction("Actualizar", "Programa", new { IdentPrograma = viewPrograma.IdentPrograma, Mensaje = viewPrograma.Mensaje });
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
        [HttpPost]
        public async Task<IActionResult> ValidarManzanaInicial(int identPrograma, int manzanaInicial, int cantidadManzanas)
        {
            var mensaje = await _programaService.ValidarManzanaInicial(identPrograma, manzanaInicial, cantidadManzanas);
            return Json(new { Mensaje = mensaje });
        }
        public async Task<IActionResult> AnularPrograma(int identPrograma)
        {
            var mensaje = await _programaService.AnularPrograma(identPrograma);
            if (mensaje == "OK")
            {
                mensaje = "Se anuló con éxito el Programa";
            }
            else
            {
                mensaje = "No se pudo anular el Programa";
            }
            return RedirectToAction("Actualizar", "Programa", new { IdentPrograma = identPrograma, Mensaje = mensaje });
        }
        [HttpPost]
        public async Task<IActionResult> ActualizarCantidadLotes(int identManzana, int nuevaCantidadLotes)
        {
            LoginResult loginResult = DatosUsuarioLogin();

            // Llamar al servicio para actualizar la cantidad de lotes
            var mensaje = await _programaService.ActualizarCantidadLotes(identManzana, nuevaCantidadLotes);

            return Json(new { mensaje });
        }
        public async Task<IActionResult> ListarManzanaJson(int identPrograma)
        {
            // Llamar al servicio para obtener la lista actualizada de manzanas
            var viewManzanas = await _programaService.ListarManzanasPrograma(identPrograma);

            // Devolver la vista parcial con los datos actualizados
            return Json(viewManzanas);
        }
        public async Task<IActionResult> EliminarPropietario(int identPrograma, int IdentPropietario)
        {
            LoginResult loginResult = DatosUsuarioLogin();
            
            var mensaje = await _programaService.AnularPropietario(IdentPropietario);
            if (mensaje == "OK")
            {
                var propietario = await _programaService.ListarPropietario(identPrograma);
                return Json(propietario);
            }
            else
            {
                mensaje = "no se pudo anular propietario";
                return Json(new { mensaje });
            }
            // Obtener el propietario recién registrado
            
            
        }
    }
}
