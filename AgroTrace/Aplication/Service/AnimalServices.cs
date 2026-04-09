using AgroTrace.Aplication.DTO;
using AgroTrace.Infrastructure.Data;
using Microsoft.Identity.Client;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AgroTrace.Aplication.Service
{
    public class AnimalServices : IAnimal
    {
        private readonly AppDbContext _context;

        public AnimalServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Response<AgregarAnimalesResponse>> AgregarAnimal(AgregarAnimalesRequest animal)
        {
           
            catch (Exception ex)
            {





            }


        }




    }
}
