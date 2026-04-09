using AgroTrace.Aplication.DTO;
using AgroTrace.Aplication.Validators.ValidationAnimal;
using FluentValidation;

namespace AgroTrace.Aplication.Validators.Animal
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
            .MaximumLength(15).WithMessage("Maximo 15 caracteres");

            RuleFor(x => x.FechaNacimiento)
            .Must(fecha => fecha != default)
            .WithMessage("La fecha de nacimiento es obligatoria")
            .LessThan(DateTime.Now)
            .WithMessage("La fecha de nacimiento no puede ser futura");

            RuleFor(x => x.FincaId)
            .GreaterThan(0).WithMessage("FincaId es obligatorio")
            .MustAsync(async (id, ct) =>
             await _metodosValidacion.FincaExiste(id))
             .WithMessage("La finca no existe");

            RuleFor(x => x.TipoAnimalId)
           .GreaterThan(0).WithMessage("TipoAnimalId es obligatorio")
           .MustAsync(async (id, ct) =>
            await _metodosValidacion.TipoAnimalExiste(id))
            .WithMessage("La finca no existe");

            RuleFor(x => x.RazaId)
           .GreaterThan(0).WithMessage("RazaId es obligatorio")
           .MustAsync(async (id, ct) =>
            await _metodosValidacion.RazaIdExiste(id))
            .WithMessage("El id Raza no existe");

            RuleFor(x => x.EstadoAnimalId)
          .GreaterThan(0).WithMessage("EstadoAnimalId es obligatorio")
          .MustAsync(async (id, ct) =>
           await _metodosValidacion.EstadoAnimalId(id))
           .WithMessage("El Id Estado Animal no existe");











        }



    }
}
