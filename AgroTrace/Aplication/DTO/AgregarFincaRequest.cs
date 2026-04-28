namespace AgroTrace.Aplication.DTO
{
    public class AgregarFincaRequest
    {
        public string Nombre { get; set; }
        public string Ubicacion {  get; set; }
        public decimal Tamaño { get; set; }
        public int UsuarioPropietarioId { get; set; }
        public bool Activo { get; set; }    



    }
}
