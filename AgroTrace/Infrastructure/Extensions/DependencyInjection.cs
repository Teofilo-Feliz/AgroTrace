using AgroTrace.Aplication.Helpers;
using AgroTrace.Aplication.Interfaces;
using AgroTrace.Aplication.Service;
using AgroTrace.Aplication.Validators;
using AgroTrace.Aplication.Validators.Usuario;
using AgroTrace.Aplication.Validators.ValidationAnimal;
using AgroTrace.Domain.Entities;
using AgroTrace.Infrastructure.PatronRepository.AnimalRepository;
using AgroTrace.Infrastructure.PatronRepository.FincaRepository;
using AgroTrace.Infrastructure.PatronRepository.GenericRepository;
using AgroTrace.Infrastructure.PatronRepository.RefreshTokenRepository;
using AgroTrace.Infrastructure.PatronRepository.RolesRepository;
using AgroTrace.Infrastructure.PatronRepository.UsuarioRepository;
using AgroTrace.Infrastructure.UnitOfWork;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace AgroTrace.Infrastructure.Extesions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();
            services.AddScoped<PasswordService>();
            services.AddScoped<JWTService>();

            services.AddScoped<IUsuario, UsuarioService>();
            services.AddScoped<ITokenGenerator, RefreshTokenService>();
            services.AddScoped<IRoles, RolesServices>();
            services.AddScoped<IUserAudi, UserAudiServices>();
            services.AddScoped<IAnimal, AnimalServices>();
            services.AddScoped<IFinca, FincaServices>();
            services.AddScoped<IEstadoAnimal, EstadoAnimalServices>();

            services.AddHttpContextAccessor();

            services.AddValidatorsFromAssemblyContaining<AgregarUsuarioValidator>();
            services.AddScoped<IValidationService, ValidationService>();
            services.AddScoped<MetodosValidacion>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, AgroTrace.Infrastructure.UnitOfWork.UnitOfWork>();

            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<IRefreshRepository, RefreshTokenRepository>();
            services.AddScoped<IAnimalRepository, AnimalRepository>();
            services.AddScoped<IFincaRepository, FincaRepository>();
            


            return services;


        }



    }
}
