using AgroTrace.Aplication.DTO;
using AgroTrace.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;

namespace AgroTrace.Aplication.Validators.Usuario
{
    public class AgregarUsuarioValidator:AbstractValidator<AgregarUsuarioRequest>
    {

        public AgregarUsuarioValidator()
        {
            RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio")
            .MaximumLength(50).WithMessage("Máximo 50 caracteres");

            RuleFor(x => x.Apellido)
                .NotEmpty().WithMessage("El apellido es obligatorio")
                .MaximumLength(50).WithMessage("Máximo 50 caracteres");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("El username es obligatorio")
                .MaximumLength(50).WithMessage("Máximo 50 caracteres")
                .MinimumLength(4).WithMessage("Mínimo 4 caracteres");

            RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es obligatorio")
            .EmailAddress().WithMessage("Formato de correo inválido")
            .MaximumLength(100).WithMessage("Máximo 100 caracteres");
            
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria")
                .MinimumLength(6).WithMessage("Debe tener al menos 6 caracteres");

            RuleFor(x => x.RolId)
                .GreaterThan(0).WithMessage("Debe seleccionar un rol válido");
        }

       



    }
}
