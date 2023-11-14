using InmobiliariaWeb.Models;
using InmobiliariaWeb.Result;

namespace InmobiliariaWeb.Interfaces
{
    public interface ILoginService
    {
        Task<LoginResult> ValidarLogin(string Usuario, string Clave);
    }
}
