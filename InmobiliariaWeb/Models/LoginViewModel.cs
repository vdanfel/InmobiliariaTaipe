using InmobiliariaWeb.Result;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace InmobiliariaWeb.Models
{
    public class LoginViewModel
    {
        
        [Required(ErrorMessage ="El campo {0} no puede estar vacío")]
        public string Usuario { get; set; }
        [Required(ErrorMessage = "El campo {0} no puede estar vacío")]
        public string Clave { get; set; }
        public string Mensaje { get; set; }
        public LoginResult LoginResult { get; set; }

    }
}
