using System.Globalization;

namespace AgroTrace.Aplication.DTO
{
    public class FincasResponse
    {
        public int id {  get; set; }
        public string Nombre { get; set; }
        public string Ubicacion { get; set; }
        public decimal Tamaño { get; set; }
        public int UsuarioPropietarioId { get; set; }
        public string NombrePropietario { get; set; } = null!;
        public bool Activo { get; set; }
    }
}
