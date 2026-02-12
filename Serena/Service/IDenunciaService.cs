using DominioSerena;
using DominioSerena.DTOs;


namespace Serena.Service
{
    public interface IDenunciaService
    {
        Task<IEnumerable<DenunciaViewModel>> GetAllByUserAsync(int userId);
        Task<DenunciaViewModel> GetByIdAsync(int id);
        Task<DenunciaViewModel> CreateAsync(DenunciaDto model);

    }
}
