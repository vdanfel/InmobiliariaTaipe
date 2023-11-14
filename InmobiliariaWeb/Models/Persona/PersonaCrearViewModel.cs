using InmobiliariaWeb.Models.Tablas;
using System.ComponentModel.DataAnnotations;

namespace InmobiliariaWeb.Models.Persona
{
    public class PersonaCrearViewModel
    {
        public int IdentPersona { get; set; }
        [Required(ErrorMessage = "Debe seleccionar un Tipo de Documento")]
        public int Ident001TipoDocumento { get; set; }
        [Required(ErrorMessage = "La seleccion {0} no puede estar en blanco")]
        public int Ident006TipoEstadoCivil { get; set; }
        [Required(ErrorMessage = "El campo {0} no puede estar en blanco")]
        public string Documento { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public string Nombres { get; set; }
        public string RazonSocial { get; set; }
        [Required(ErrorMessage = "El campo {0} no puede estar en blanco")]
        public string Celular { get; set; }
        public string Telefono { get; set; }
        [Required(ErrorMessage = "El campo {0} no puede estar en blanco")]
        public string Correo { get; set; }
        public string Contacto { get; set; }
        public string TelefonoContacto { get; set; }
        public string CelularContacto { get; set; }
        public string CorreoContacto { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public List<TipoDocumento>TipoDocumento { get; set; }
        public List<TipoEstadoCivil>TipoEstadoCivil { get; set; }
        [Required(ErrorMessage = "El campo {0} no puede estar en blanco")]
        public string Departamento { get; set; }
        public List<Departamento>ListDepartamento { get; set; }
        [Required(ErrorMessage = "El campo {0} no puede estar en blanco")]
        public string Provincia { get; set; }
        public List<Provincia>ListProvincia { get; set;}
        [Required(ErrorMessage = "El campo {0} no puede estar en blanco")]
        public string Distrito { get; set; }
        public List<Distrito>ListDistrito { get; set; }
        [Required (ErrorMessage ="El campo {0} no puede estar en blanco")]
        public string Direccion { get; set; }
        public string Referencia { get; set; }
        public string Mensaje { get; set; }
    }
}
