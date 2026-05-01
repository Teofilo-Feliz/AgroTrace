using AgroTrace.Aplication.DTO;using AgroTrace.Aplication.Helpers;using AgroTrace.Aplication.Interfaces;using AgroTrace.Domain.Entities;using AgroTrace.Infrastructure.PatronRepository.GenericRepository;using AgroTrace.Infrastructure.UnitOfWork;using System;namespace AgroTrace.Aplication.Service{    public class EstadoAnimalServices: IEstadoAnimal    {        private readonly IUnitOfWork _unitOfWork;        private readonly IValidationService _validation;        private readonly IRepository<EstadoAnimal> _repository;               public EstadoAnimalServices(IUnitOfWork unitOfWork, IValidationService validation, IRepository<EstadoAnimal> repository)        {            _unitOfWork = unitOfWork;            _validation = validation;            _repository = repository;                    }        public async Task<Response<EstadoAnimalResponse>> ObtenerEstadoAnimal(Filtro filtro)
        {
            var response = new Response<EstadoAnimalResponse>();

            try
            {
                var (estadoAnimal, total) = await _repository.Obtener(
                    filtro,
                    e => string.IsNullOrEmpty(filtro.Buscar) || e.Nombre.Contains(filtro.Buscar),
                    e => e.Id
                );

                var data = estadoAnimal.Select(e => new EstadoAnimalResponse
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Activo = e.Activo
                }).ToList();

                response.Successful = true;
                response.DataList = data;
                response.EntityId = total;
                response.Message = data.Any()
                    ? "Estados obtenidos exitosamente"
                    : "No hay estados registrados";

                return response;
            }
            catch (Exception ex)
            {
                response.Successful = false;
                response.Message = "Error al obtener estados";
                response.Errors.Add(ex.InnerException?.Message ?? ex.Message);
                return response;
            }
        }        public async Task<Response<AgregarEstadoAnimalResponse>> AgregarEstadoAnimal(AgregarEstadoAnimalRequest request)        {            var response = new Response <AgregarEstadoAnimalResponse>();            var errores = new List<string>();            try            {                try                {                    await _validation.ValidateAsync(request);                }                catch (FluentValidation.ValidationException ex)                {                    errores.AddRange(ex.Errors.Select(e => e.ErrorMessage));                }                var nombre = StringNormalizer.Normalize(request.Nombre);                var existe = await _repository.Exists(e => e.Nombre.ToLower() == nombre);                if (existe)                {                    errores.Add("El nombre del estado ya existe");                 }                if (errores.Any())                {                    response.Successful = false;                    response.Message = "Errores de validación";                    response.Errors = errores;                    return response;                }                var entity = new EstadoAnimal                {                    Nombre = request.Nombre,                    Activo = request.Activo,                };                await _repository.AddAsync(entity);                await _unitOfWork.SaveChangesAsync();                response.Successful = true;                response.Message = "Estado Animal agregado exitosamente";                response.Data = new AgregarEstadoAnimalResponse                {                    Id = entity.Id,                    Nombre = entity.Nombre,                    Activo = entity.Activo,                };                return response;            }            catch (Exception ex)            {                response.Successful = false;                response.Message = "Error al agregar el estado animal";                response.Errors.Add(ex.InnerException?.Message ?? ex.Message);                return response;            }        }    }}