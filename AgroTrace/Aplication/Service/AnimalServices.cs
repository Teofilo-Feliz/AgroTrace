using AgroTrace.Aplication.DTO;
using AgroTrace.Aplication.Validators.Animal;
using AgroTrace.Infrastructure.Data;
using Microsoft.Identity.Client;
using AgroTrace.Aplication.Validators;
using AgroTrace.Domain.Entities;
using AgroTrace.Aplication.Validators.ValidationAnimal;
using AgroTrace.Aplication.Interfaces;



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

            try
            {
                

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
                response.Message = "Animal Agregado exitosamente";
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
            catch (FluentValidation.ValidationException ex)
            {
                response.Successful = false;
                response.Message = "Error al agregar Animal";
                response.Errors.Add(ex.InnerException?.Message ?? ex.Message);

                return response;
               
            }
           
           



        }




    }
}
