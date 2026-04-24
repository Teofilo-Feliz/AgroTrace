namespace AgroTrace.Aplication.DTO
{
    public class Filtro
    {
          
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Buscar { get; set; }
        public bool? Activo { get; set; }
        public string OrdenarPor { get; set; } = "Nombre";
        public bool Descendente { get; set; } = false;
        
    }
}
