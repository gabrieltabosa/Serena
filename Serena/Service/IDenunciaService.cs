using Serena.Models;
using Serena.Models.DTOs;


namespace Serena.Service
{
    public interface IDenunciaService
    {
        Task<IEnumerable<DenunciaViewModel>> GetAllByUserAsync(int userId);
        Task<DenunciaViewModel> GetByIdAsync(int id);
        Task<DenunciaViewModel> CreateAsync(DenunciaDto model);

    }
}
