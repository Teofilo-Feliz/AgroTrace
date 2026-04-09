using FluentValidation;

namespace AgroTrace.Aplication.Validators
{
    public class ValidationService : IValidationService
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
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
