using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models;
using InmobiliariaWeb.Models.Persona;
using InmobiliariaWeb.Result;
using InmobiliariaWeb.Result.Persona;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Runtime.CompilerServices;

namespace InmobiliariaWeb.Controllers
{
    public class PersonaController:Controller
    {
        private readonly ITablasService _tablasService;
        private readonly IPersonaService _personaService;

        public PersonaController(ITablasService tablasService, IPersonaService personaService)
        { 
            _tablasService = tablasService;
            _personaService = personaService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            PersonaViewModel personaViewModel = new PersonaViewModel();
            personaViewModel.PersonaList = await _personaService.PersonaBandeja(personaViewModel.Buscar);
            return View(personaViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Index(PersonaViewModel personaViewModel, string Buscar)
        {
            personaViewModel.PersonaList = await _personaService.PersonaBandeja(Buscar);
            return View(personaViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Crear( string mensaje)
        {
            PersonaCrearViewModel personaCrearViewModel = new PersonaCrearViewModel();
            personaCrearViewModel.TipoDocumento = await _tablasService.ListarTipoDocumento();
            personaCrearViewModel.TipoEstadoCivil = await _tablasService.ListarTipoEstadoCivil();
            personaCrearViewModel.ListDepartamento = await _tablasService.ListarDepartamento();
            if (mensaje != null)
            {
                personaCrearViewModel.Mensaje = mensaje;
                
            }
            return View(personaCrearViewModel);

        }
        [HttpGet]
        public async Task<IActionResult> Actualizar(int identPersona, string mensaje)
        {

            PersonaList personaList = new PersonaList();
            personaList = await _personaService.Persona_XIdentPersona(identPersona);
            personaList.TipoEstadoCivil= await _tablasService.ListarTipoEstadoCivil();
            personaList.ListDepartamento = await _tablasService.ListarDepartamento();
            if (mensaje != null) 
            {
                personaList.Mensaje = mensaje;
            }
            return View(personaList);
        }
        [HttpPost]
        public async Task<IActionResult> PersonaActualizar(PersonaList personaList)
        {
            LoginResult loginResult = new LoginResult();
            loginResult = DatosUsuarioLogin();
            personaList = await _personaService.PersonaActualizar(personaList, loginResult);
            return RedirectToAction("Actualizar", "Persona", new {identPersona = personaList.Ident_Persona,mensaje = personaList.Mensaje });
            //return RedirectToAction("Actualizar", personaList);
        }
        public async Task<IActionResult> PersonaAnular(int ident_Persona)
        {
            string mensaje = "";
            LoginResult loginResult = new LoginResult();
            loginResult = DatosUsuarioLogin();
            mensaje = await _personaService.PersonaAnular(ident_Persona, loginResult);
            return RedirectToAction("Actualizar", "Persona", new { identPersona = ident_Persona, mensaje = mensaje });
        }
        [HttpPost]
        public async Task<IActionResult> PersonaRegistrar(PersonaCrearViewModel personaCrearViewModel)
        {
            int existe = await _personaService.PersonaExiste(personaCrearViewModel);
            if (existe == 1)
            {
                personaCrearViewModel.Mensaje = "El documento ya existe";
                return RedirectToAction("Crear", personaCrearViewModel);
            }
            else
            {
                LoginResult loginResult = new LoginResult();
                loginResult = DatosUsuarioLogin();
                PersonaResult personaResult = new PersonaResult();
                personaResult = await _personaService.PersonaRegistrar(personaCrearViewModel, loginResult);
                return RedirectToAction("Crear", "Persona", new { mensaje = personaResult.Mensaje });
            }
            
        }
        public async Task<IActionResult> CargarProvincias(string departamento)
        {
            var provinciaList = await _tablasService.ListarProvincia(departamento);
            return Json(provinciaList);
        }
        public async Task<IActionResult> CargarDistritos(string departamento, string provincia)
        {
            var distritoLists = await _tablasService.ListarDistrito(departamento, provincia);
            return Json(distritoLists);
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
