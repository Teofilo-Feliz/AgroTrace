using Microsoft.VisualBasic;

namespace AgroTrace.Aplication.DTO
{
    public class AgregarAnimalesResponse
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Sexo { get; set; }
        public decimal Peso  { get; set; }
        public int FincaId { get; set; }
        public int TipoAnimalId { get; set; }
        public int RazaId { get; set; }
        public int EstadoAnimalId { get; set; }
        public bool Activo { get; set; }




    }
}
