using InmobiliariaWeb.Models.Tablas;

namespace InmobiliariaWeb.Models.Programa
{
    public class ProgramaCrearViewModel
    {
        public int IdentPrograma { get; set; }
        public string NombrePrograma { get; set; }
        public string NumeroPartida { get; set; }
        public string Direccion { get; set; }
        public string Referencia { get; set; }
        public decimal AreaTotal { get; set; }
        public decimal AreaLotizada { get; set; }
        public int CantidadManzanas { get; set; }
        public string Suministro { get; set; }
        public string Mensaje { get; set; }
        public ViewPrograma ViewPrograma { get; set; }
        public List<Manzanas> Manzanas { get; set; }
        public int ManzanaInicial { get; set; }
    }
}
