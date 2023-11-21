using InmobiliariaWeb.Models.Programa;
using InmobiliariaWeb.Result;
using InmobiliariaWeb.Result.Programa;

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
        Task<List<ViewManzana>> ListarManzanasPrograma(int ident_Programa);
        Task<string> ValidarManzanaInicial(int Ident_Programa, int ManzanaInicial, int CantidadManzanas);
        Task<string> AnularPrograma(int Ident_Programa);
        Task<string> ActualizarPrograma(ViewPrograma viewPrograma, LoginResult loginResult);
        Task<string> AnularManzanasList(int IdentPrograma, int IdentUsuario);
        Task<string> ActualizarCantidadLotes(int IdentManzana, int CantidadLotes);
        Task<string> AnularPropietario(int IdentPropietario);
    }
}
