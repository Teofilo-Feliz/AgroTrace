namespace AgroTrace.Aplication.DTO
{
    public class AgregarFincaResponse
    {
        public int Id { get; set; }
        public string Nombre{ get; set; }
        public string Ubicacion{ get; set; }
        public decimal Tamaño{ get; set; }
        public int UsuarioPropiedadId { get; set; }
        public string UsuarioPropiedadNombre { get; set; } = string.Empty;
        public bool activo { get; set; }


    }
}
