using AgroTrace.Aplication.DTO;
using AgroTrace.Domain.Entities;
using FluentValidation;

namespace AgroTrace.Aplication.Validators.ValidationAnimal
{
    public class AgregarAnimalValidator: AbstractValidator<AgregarAnimalesRequest>
    {
        private readonly MetodosValidacion _metodosValidacion;

        public AgregarAnimalValidator(MetodosValidacion metodosValidacion)
        {
            _metodosValidacion = metodosValidacion;




            RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio")
            .MaximumLength(100).WithMessage("Maximo 100 caracteres");

            RuleFor(x => x.Codigo)
            .NotEmpty().WithMessage("El codigo es obligatorio")
            .MaximumLength(100).WithMessage("Maximo 100 caracteres");

            RuleFor(x => x.Sexo)
           .NotEmpty().WithMessage("El sexo es obligatorio")
           .Must(x => x == "M" || x == "H")
           .WithMessage("El sexo debe ser M o H");

            RuleFor(x => x.FechaNacimiento)
            .Must(fecha => fecha != default)
            .WithMessage("La fecha de nacimiento es obligatoria")
            .LessThan(DateTime.UtcNow)
            .WithMessage("La fecha de nacimiento no puede ser futura");

            RuleFor(x => x.FincaId)
           .Cascade(CascadeMode.Stop)
           .GreaterThan(0).WithMessage("FincaId es obligatorio")
           .MustAsync(async (id, ct) =>
            await _metodosValidacion.ExisteAsync<Finca>(f => f.Id == id))
           .WithMessage("La finca no existe");

            RuleFor(x => x.TipoAnimalId)
           .Cascade(CascadeMode.Stop)
           .GreaterThan(0).WithMessage("TipoAnimalId es obligatorio")
           .MustAsync(async (id, ct) =>
            await _metodosValidacion.ExisteAsync<TipoAnimal>(f => f.Id == id))
            .WithMessage("El Id tipo animal no existe");

            RuleFor(x => x.RazaId)
           .Cascade(CascadeMode.Stop)
           .GreaterThan(0).WithMessage("RazaId es obligatorio")
           .MustAsync(async (id, ct) =>
            await _metodosValidacion.ExisteAsync<Raza>(f => f.Id == id))
            .WithMessage("El id Raza no existe");

            RuleFor(x => x.EstadoAnimalId)
          .Cascade(CascadeMode.Stop)
          .GreaterThan(0).WithMessage("EstadoAnimalId es obligatorio")
          .MustAsync(async (id, ct) =>
           await _metodosValidacion.ExisteAsync<EstadoAnimal>(f => f.Id == id))
           .WithMessage("El Id Estado Animal no existe");











        }



    }
}
