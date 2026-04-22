using AgroTrace.Aplication.DTO;
using AgroTrace.Aplication.Interfaces;
using AgroTrace.Aplication.Validators;
using AgroTrace.Aplication.Validators.Animal;
using AgroTrace.Aplication.Validators.ValidationAnimal;
using AgroTrace.Domain.Entities;
using AgroTrace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;



namespace AgroTrace.Aplication.Service
{
    public class AnimalServices : IAnimal
    {
        private readonly AppDbContext _context;
        private readonly AgregarAnimalValidator _validator;
        private readonly MetodosValidacion _metodos;

        public AnimalServices(AppDbContext context, AgregarAnimalValidator validator, MetodosValidacion metodos)
        {
            _context = context;
            _validator = validator;
            _metodos = metodos;

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

                var existeCodigo = await _context.Animales
                .AnyAsync(a =>
                 a.Codigo.ToLower() == animal.Codigo.ToLower() &&
                 a.FincaId == animal.FincaId
     );

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

                _context.Animales.Add(entity);
                await _context.SaveChangesAsync();

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



    }
}
