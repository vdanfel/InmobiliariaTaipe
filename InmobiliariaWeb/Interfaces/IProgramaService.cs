using InmobiliariaWeb.Models.Programa;
using InmobiliariaWeb.Result;
using InmobiliariaWeb.Result.Persona;

namespace InmobiliariaWeb.Interfaces
{
    public interface IProgramaService
    {
        Task<int> RegistrarPrograma(ViewPrograma viewPrograma, LoginResult loginResult);
        Task<string> RegistrarManzanas(ViewPrograma viewPrograma, LoginResult loginResult);
        Task<ViewPrograma> BuscarProgramaIdentPrograma(int identPrograma);
        Task<List<ProgramaList>> BandejaPrograma(string buscar);
        Task<int> RegistrarPropietario(int identPrograma, int identPersona, int ident011TipoPropietario, string numeroPartida, int identUsuario);
        Task<List<ViewPropietario>> ListarPropietario(int identPrograma);
    }
}
