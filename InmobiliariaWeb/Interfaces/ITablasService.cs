using InmobiliariaWeb.Models.Tablas;

namespace InmobiliariaWeb.Interfaces
{
    public interface ITablasService
    {
        Task<List<TipoEstadoCivil>> ListarTipoEstadoCivil();
        Task<List<TipoDocumento>> ListarTipoDocumento();
        Task<List<Departamento>> ListarDepartamento();
        Task<List<Provincia>> ListarProvincia(string codigoDepartamento);
        Task<List<Distrito>> ListarDistrito(string codigoDepartamento, string codigoProvincia);
        Task<List<TipoPropietario>> ListarTipoPropietario();
        Task<List<Manzanas>> ListarManzanas();
    }
}
