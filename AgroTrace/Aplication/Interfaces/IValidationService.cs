namespace AgroTrace.Aplication.Interfaces
{
    public interface IValidationService
    {
        Task ValidateAsync<T>(T model);
    }
}