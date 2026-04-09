
namespace AgroTrace.Aplication.Validators
{
    public interface IValidationService
    {
        Task ValidateAsync<T>(T model);
    }
}