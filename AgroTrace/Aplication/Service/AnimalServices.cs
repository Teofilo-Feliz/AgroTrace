using AgroTrace.Aplication.DTO;
using AgroTrace.Aplication.Interfaces;
using AgroTrace.Aplication.Validators.ValidationAnimal;
using AgroTrace.Domain.Entities;
using AgroTrace.Infrastructure.PatronRepository.AnimalRepository;
using AgroTrace.Infrastructure.UnitOfWork;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace AgroTrace.Aplication.Service
{
    public class AnimalServices : IAnimal
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AgregarAnimalValidator _validator;
        private readonly MetodosValidacion _metodos;
        private readonly IValidationService _validation;

        public AnimalServices(IAnimalRepository animalRepository, AgregarAnimalValidator validator, MetodosValidacion metodos,
            IUnitOfWork unitOfWork, IValidationService validation)
        {
            _animalRepository = animalRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _metodos = metodos;
            _validation = validation;
        }


        public async Task<Response<ObtenerAnimalResponse>> ObtenerAnimales(FiltroAnimal filtro)
        {
            var response = new Response<ObtenerAnimalResponse>();

            try
            {

                var (animales, total) = await _animalRepository.ObtenerAnimales(filtro);


                var data = animales.Select(a => new ObtenerAnimalResponse
                {
                    Id = a.Id,
                    Codigo = a.Codigo,
                    Nombre = a.Nombre,
                    FechaNacimiento = a.FechaNacimiento,
                    Sexo = a.Sexo,
                    Peso = a.Peso,
                    FincaId = a.FincaId,
                    FincaNombre = a.Finca.Nombre,
                    TipoAnimalId = a.TipoAnimalId,
                    TipoAnimalNombre = a.TipoAnimal.Nombre,
                    RazaId = a.RazaId,
                    RazaNombre = a.Raza.Nombre,
                    EstadoAnimalId = a.EstadoAnimalId,
                    EstadoAnimalNombre = a.EstadoAnimal.Nombre,
                    Activo = a.Activo
                }).ToList();

                response.Successful = true;
                response.DataList = data;
                response.EntityId = total;
                response.Message = data.Any()
                    ? "Animales obtenidos exitosamente"
                    : "No hay animales registrados";

                return response;
            }
            catch (Exception ex)
            {
                response.Successful = false;
                response.Message = "Error al obtener los animales";
                response.Errors.Add(ex.InnerException?.Message ?? ex.Message);
                return response;
            }
        }




        public async Task<Response<AgregarAnimalesResponse>> AgregarAnimal(AgregarAnimalesRequest animal)
        {
            var response = new Response<AgregarAnimalesResponse>();
            var errores = new List<string>();

            try
            {
                var result = await _validator.ValidateAsync(animal);


                if (!result.IsValid)
                {
                    return new Response<AgregarAnimalesResponse>
                    {
                        Successful = false,
                        Message = "Errores de validación",
                        Errors = result.Errors.Select(e => e.ErrorMessage).ToList()
                    };
                }

                var existeCodigo = await _animalRepository.ExisteCodigo(animal.Codigo, animal.FincaId);
             

                if (existeCodigo)
                {
                    errores.Add($"El código {animal.Codigo} ya existe en la finca {animal.FincaId}");
                }


                if (errores.Any())
                {
                    return new Response<AgregarAnimalesResponse>
                    {
                        Successful = false,
                        Message = "Errores de validación",
                        Errors = errores
                    };
                }

                var entity = new Animal
                {
                    Codigo = animal.Codigo,
                    Nombre = animal.Nombre,
                    FechaNacimiento = animal.FechaNacimiento,
                    Sexo = animal.Sexo,
                    Peso = animal.Peso,
                    FincaId = animal.FincaId,
                    TipoAnimalId = animal.TipoAnimalId,
                    RazaId = animal.RazaId,
                    EstadoAnimalId = animal.EstadoAnimalId,
                    Activo = animal.Activo,
                };

                await _animalRepository.AgregarAnimal(entity);
                await _unitOfWork.SaveChangesAsync();

                response.Successful = true;
                response.Message = "Animal agregado exitosamente";
                response.Data = new AgregarAnimalesResponse
                {
                    Id = entity.Id,
                    Nombre = entity.Nombre,
                    FechaNacimiento = entity.FechaNacimiento,
                    Sexo = entity.Sexo,
                    Peso = entity.Peso,
                    FincaId = entity.FincaId,
                    TipoAnimalId = entity.TipoAnimalId,
                    RazaId = entity.RazaId,
                    EstadoAnimalId = entity.EstadoAnimalId,
                    Activo = entity.Activo,
                };

                return response;
            }
            catch (Exception ex)
            {
                response.Successful = false;
                response.Message = "Error al agregar Animal";
                response.Errors.Add(ex.InnerException?.Message ?? ex.Message);

                return response;
            }
        }

        public async Task<Response<ActualizarAnimalResponse>> ActualizarAnimal(int id, ActualizarAnimalRequest animal)
        {
            var response = new Response<ActualizarAnimalResponse>();
            var errores = new List<string>();

            try
            {

                try
                {
                    await _validation.ValidateAsync(animal);
                }
                catch (FluentValidation.ValidationException ex)
                {
                    errores.AddRange(ex.Errors.Select(e => e.ErrorMessage));
                }

                var entity = await _animalRepository.ActualizarAnimal(id);

                if (entity == null)
                {
                    errores.Add($"Animal con Id {id} no encontrado");
                   
                }
                if (animal.FechaNacimiento == null)
                {
                    errores.Add  ("La fecha de nacimiento es obligatoria");
                }

                var existeCodigo = await _animalRepository.ExisteCodigoActualizar(animal.Codigo, animal.FincaId, id);
                    
                if (existeCodigo)
                {
                    errores.Add($"Este codigo {animal.Codigo} ya existe en la finca {animal.FincaId}");
                }

                if (errores.Any())
                {
                    response.Successful = false;
                    response.Message = "Errores de validaciones";
                    response.Errors = errores;
                    return response;
                }

                entity.Codigo = animal.Codigo.Trim();
                entity.Nombre = animal.Nombre.Trim();
                entity.FechaNacimiento = animal.FechaNacimiento.Value;
                entity.Sexo = animal.Sexo;
                entity.Peso = animal.Peso;
                entity.FincaId= animal.FincaId;
                entity.TipoAnimalId = animal.TipoAnimalId;
                entity.RazaId = animal.RazaId;
                entity.EstadoAnimalId = animal.EstadoAnimalId;
                entity.Activo = animal.Activo;

                await _unitOfWork.SaveChangesAsync();

                response.Successful= true;
                response.Message = "Animal Actualizado exitosamente";
                response.Data = new ActualizarAnimalResponse
                {
                    Id = entity.Id,
                    Codigo = entity.Codigo,
                    Nombre = entity.Nombre,
                    FechaNacimiento = entity.FechaNacimiento,
                    Sexo = entity.Sexo,
                    Peso = entity.Peso,
                    FincaId = entity.FincaId,
                    TipoAnimalId = entity.TipoAnimalId,
                    RazaId = entity.RazaId,
                    EstadoAnimalId = entity.EstadoAnimalId,
                    FechaModificacion = entity.FechaModificacion,
                    UsuarioModificacion = entity.UsuarioModificacion,
                    Activo = entity.Activo,

                };
                return response;

            }
            catch (Exception ex)
            {
                response.Successful = false;
                response.Message = "Error al actualizar el Animal";
                response.Errors.Add(ex.InnerException?.Message ?? ex.Message);
                return response;

            }
        }



    }
}
