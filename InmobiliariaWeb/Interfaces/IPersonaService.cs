using InmobiliariaWeb.Models;
using InmobiliariaWeb.Models.Persona;
using InmobiliariaWeb.Result;
using InmobiliariaWeb.Result.Persona;

namespace InmobiliariaWeb.Interfaces
{
    public interface IPersonaService
    {
        Task<PersonaResult> PersonaRegistrar(PersonaCrearViewModel personaCrearViewModel, LoginResult loginResult);
        Task<int> PersonaExiste(PersonaCrearViewModel personaCrearViewModel);
        Task<List<PersonaList>> PersonaBandeja(string buscar);
        Task<PersonaList> Persona_XIdentPersona(int identPersona);
        Task<PersonaList> PersonaActualizar(PersonaList personaList, LoginResult loginResult);
        Task<string> PersonaAnular(int ident_Persona, LoginResult loginResult);
    }
}
