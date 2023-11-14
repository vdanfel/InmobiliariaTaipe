using InmobiliariaWeb.Models.Tablas;

namespace InmobiliariaWeb.Result.Persona
{
    public class PersonaList
    {
        public int Indice { get; set; }
        public int Ident_Persona { get; set; }
        public int Ident_001_TipoDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public string Documento { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public string Nombre { get; set; }
        public int Ident006TipoEstadoCivil { get; set; }
        public string EstadoCivil { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string RazonSocial { get; set; }
        public string NombreCompleto { get; set; }
        public string Celular { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Contacto { get; set; }
        public string TelefonoContacto { get; set; }
        public string CorreoContacto { get; set; }
        public int Ident_Ubigeo { get; set; }
        public string CodigoDepartamento { get; set; }
        public string CodigoProvincia { get; set; }
        public string CodigoDistrito { get; set; }
        public string Direccion { get; set; }
        public string Referencia { get; set; }
        public int Ident004Estado { get; set; }
        public string Estado { get; set; }
        public List<TipoEstadoCivil> TipoEstadoCivil { get; set; }
        public List<Departamento> ListDepartamento { get; set; }
        public List<Provincia> ListProvincia { get; set; }
        public List<Distrito> ListDistrito { get; set; }
        public string Mensaje { get; set; }
    }
}
