using AgroTrace.Aplication.DTO;
using AgroTrace.Aplication.Interfaces;
using AgroTrace.Domain.Entities;
using AgroTrace.Infrastructure.PatronRepository.FincaRepository;
using AgroTrace.Infrastructure.PatronRepository.GenericRepository;
using AgroTrace.Infrastructure.UnitOfWork;

namespace AgroTrace.Aplication.Service
{
    public class FincaServices: IFinca
    {
        private IFincaRepository _fincaRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IValidationService _validation;
        private readonly IRepository<Finca> _repository;

        public FincaServices(IUnitOfWork unitOfWork, IFincaRepository fincaRepository, IValidationService validation, IRepository<Finca> repository )
        {
            _unitOfWork = unitOfWork;
            _fincaRepository = fincaRepository;
            _validation = validation;
            _repository = repository;
        }


        public async Task<Response<FincasResponse>> ObtenerFincas(Filtro filtro)
        {
            var response = new Response<FincasResponse>();
           

            try
            {
                var (fincas, total) = await _fincaRepository.ObtenerFincas(filtro);



                var data = fincas.Select(f => new FincasResponse
                {
                    id = f.Id,
                    Nombre = f.Nombre,
                    Ubicacion = f.Ubicacion,
                    Tamaño = f.Tamaño,
                    UsuarioPropietarioId = f.UsuarioPropietarioId,
                    NombrePropietario = f.UsuarioPropietario.Nombre,
                    Activo = f.Activo,
                }).ToList();

                response.Successful = true;
                response.DataList = data;
                response.EntityId = total;
                response.Message = data.Any()
                    ? "Fincas obtenidos exitosamente"
                    : "No hay Fincas registrados";
                return response;

            }
            catch (Exception ex)
            {
                response.Successful = false;
                response.Message = "Error al obtener Fincas";
                response.Errors.Add(ex.InnerException?.Message ?? ex.Message);
                return response;

            }



        }



        public async Task<Response<AgregarFincaResponse>> AgregarFinca(AgregarFincaRequest finca)
        {
            var response = new Response<AgregarFincaResponse>();
            var errores = new List<string>();

            try
            {
                try
                {
                    await _validation.ValidateAsync(finca);
                }
                catch (FluentValidation.ValidationException ex)
                {

                    errores.AddRange(ex.Errors.Select(e => e.ErrorMessage));
                }

                var usuarioExiste = await _fincaRepository.ExisteUsuario(finca.UsuarioPropietarioId);

                if (!usuarioExiste)
                {
                    errores.Add("El usuario propietario no existe, por favor ingrese uno que exista");
                }

                var fincaExiste = await _fincaRepository.FincaExiste(finca.Nombre, finca.UsuarioPropietarioId);

                if (fincaExiste)
                {
                    errores.Add("Esta finca ya existe bajo este usuario propietario"); 
                }

                if (errores.Any())
                {
                    response.Successful = false;
                    response.Message = "Errores de validación";
                    response.Errors = errores;
                    return response;
                }

                var entity = new Finca
                {
                    Nombre = finca.Nombre,
                    Ubicacion = finca.Ubicacion,
                    Tamaño = finca.Tamaño,
                    UsuarioPropietarioId = finca.UsuarioPropietarioId,
                    Activo = finca.Activo,
                };
                await _fincaRepository.AgregarFinca(entity);
                await _unitOfWork.SaveChangesAsync();

                var nombreUsuario = await _fincaRepository
                     .ObtenerNombreUsuarioPropiedad(finca.UsuarioPropietarioId);

                response.Successful = true;
                response.Message = "Finca agregada exitosamente";
                response.Data = new AgregarFincaResponse
                {
                    Id = entity.Id,
                    Nombre = entity.Nombre,
                    Ubicacion = entity.Ubicacion,
                    Tamaño = entity.Tamaño,
                    UsuarioPropiedadId = entity.UsuarioPropietarioId,
                    UsuarioPropiedadNombre = nombreUsuario!,
                    activo = entity.Activo,
                };

                return response;


            }
            catch (Exception ex)
            {
                response.Successful = false;
                response.Message = "Error al agregar la finca";
                response.Errors.Add(ex.InnerException?.Message ?? ex.Message);
                return response;

            }



        }

        public async Task<Response<ActualizarFincaResponse>> ActualizarFinca(ActualizarFincaRequest finca, int id)
        {
            var response = new Response<ActualizarFincaResponse>();
            var errores = new List<string>();

            try
            {
                try
                {
                    await _validation.ValidateAsync(finca);
                }
                catch (FluentValidation.ValidationException ex)
                {
                    errores.AddRange(ex.Errors.Select(e => e.ErrorMessage));
                }

                var entity = await _repository.GetByIdAsync(id);

                if (entity == null)
                {
                    errores.Add($"Finca con id {id} no encontrada");
                }
               
                var fincaExiste = await _fincaRepository.FincaExiste(finca.Nombre, finca.UsuarioPropietarioId);

                if (fincaExiste)
                {
                    errores.Add("Esta finca ya existe bajo este usuario propietario");
                }


                if (errores.Any())
                {
                    response.Successful = false;
                    response.Message = "Errores de validación";
                    response.Errors = errores;
                    return response;
                }

                entity.Nombre = finca.Nombre;
                entity.Ubicacion = finca.Ubicacion;
                entity.Tamaño = finca.Tamaño;
                entity.UsuarioPropietarioId = finca.UsuarioPropietarioId;
                entity.Activo = finca.Activo;

                _repository.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var nombreUsuario = await _fincaRepository
                   .ObtenerNombreUsuarioPropiedad(finca.UsuarioPropietarioId);

                response.Successful = true;
                response.Message = "Finca actualizada correctamente";
                response.Data = new ActualizarFincaResponse
                {
                    Id = entity.Id,
                    Nombre = entity.Nombre,
                    Ubicacion = entity.Ubicacion,
                    Tamaño = entity.Tamaño,
                    UsuarioPropietarioId = entity.UsuarioPropietarioId,
                    NombrePropietario = nombreUsuario!,
                    UsuarioModificacion = entity.UsuarioModificacion!,
                    FechaModificacion = entity.FechaModificacion,
                    Activo = entity.Activo
                };

                return response;
            }
            catch (Exception ex)
            {
                response.Successful = false;
                response.Message = "Error al actualizar la finca";
                response.Errors.Add(ex.InnerException?.Message ?? ex.Message);
                return response;
            }
        }
    }
 }

