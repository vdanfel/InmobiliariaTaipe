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
    }
}
