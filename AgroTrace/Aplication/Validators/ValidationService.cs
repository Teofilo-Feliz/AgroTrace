using AgroTrace.Aplication.Interfaces;
using AgroTrace.Aplication.Validators.ValidationAnimal;
using FluentValidation;

namespace AgroTrace.Aplication.Validators
{
    public class ValidationService : IValidationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly MetodosValidacion _metodos;

        public ValidationService(IServiceProvider serviceProvider, MetodosValidacion metodos)
        {
            _serviceProvider = serviceProvider;
            _metodos = metodos;
        }

        public async Task ValidateAsync<T>(T model)
        {
            var validator = _serviceProvider.GetService<IValidator<T>>();

            if (validator == null)
                return;

            var result = await validator.ValidateAsync(model);

            if (!result.IsValid)
            {
                var errors = result.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();

                throw new ValidationException(result.Errors);
            }
        }
    }
}
