using DominioSerena;
using InfrastructureSerena;

using InfrastructureSerena;
using Microsoft.Extensions.Logging;

namespace ServiceSerena
{
    public interface IAtivoService : IGenericService<Ativo>
    {
        Task<IEnumerable<Ativo>> GetAtivosByUserIdAsync(int userId);

        // RB2: Método para buscar Ativos pelo ID da Categoria (Exemplo)
        Task<IEnumerable<Ativo>> GetAtivosByCategoriaIdAsync(int categoriaId);

        // RB3: Lógica de Negócio pura
        Task<IEnumerable<Ativo>> GetAtivosDisponiveisParaTransferenciaAsync();

        Task<IEnumerable<Ativo>> GetAtivosByCategoriaIdAndUserIdAsync(int categoriaId, int userId);
    }
}
