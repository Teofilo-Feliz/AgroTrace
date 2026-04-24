using AgroTrace.Aplication.DTO;
using AgroTrace.Domain.Entities;
using AgroTrace.Infrastructure.Data;
using AgroTrace.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace AgroTrace.Infrastructure.PatronRepository.UsuarioRepository
{
    public class UsuarioRepository:IUsuarioRepository
    {
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public UsuarioRepository(AppDbContext context,IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<(List<Usuario>, int)> ObtenerUsuarios(Filtro filtro)
        {
            var query = _context.Usuarios
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro.Buscar))
            {
                query = query.Where(r => r.Nombre.Contains(filtro.Buscar));
            }

            query = filtro.Descendente
               ? query.OrderByDescending(r => r.Id)
               : query.OrderBy(r => r.Id);


            var total = await query.CountAsync();

            var data = await query
                .Skip((filtro.PageNumber - 1) * filtro.PageSize)
                .Take(filtro.PageSize)
                .ToListAsync();

            return (data, total);
        }

        public async Task<bool> UsernameExiste(string username)
        {
            return await _context.Usuarios.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> EmailExiste(string email)
        {
            return await _context.Usuarios.AnyAsync(u => u.Email == email);
        }

        public async Task<Usuario?> ObtenerUsuarioPorId(int id)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Id == id);
        }

        
     
        public async Task AgregarUsuario(Usuario usuario)
        {
            var agregarUsuario = _context.AddAsync(usuario);
            await _unitOfWork.SaveChangesAsync();
   
        }

        public async Task<Usuario?> ActualizarUsuario(int id)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Usuario?> LogueoUsuario(string username)
        {
            return await _context.Usuarios
                   .Include(u => u.Rol)
                   .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
            
        }
    }
}

