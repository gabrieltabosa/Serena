using Microsoft.AspNetCore.Mvc;
using ServiceSerena;
using System.Net;
using DominioSerena;

namespace SerenaApi.Controllers
{
    public class AtivosController: ControllerBase
    {
        // Injeção de Dependência da interface do Serviço
        private readonly IAtivoService _ativoService;

        public AtivosController(IAtivoService ativoService)
        {
            _ativoService = ativoService;
        }

        // --- CRUD: CREATE (POST) ---
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Post([FromBody] Ativo ativo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _ativoService.AddAsync(ativo);
                // Retorna 201 Created com a rota para buscar o novo recurso.
                return CreatedAtAction(nameof(GetById), new { id = ativo.Id }, ativo);
            }
            catch (InvalidOperationException ex)
            {
                // Captura regras de negócio violadas (ex: DataAquisicao futura)
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro interno ao processar a criação do ativo.");
            }
        }

        // --- CRUD: READ (GET) ---

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Ativo>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var ativos = await _ativoService.GetAllAsync();
            return Ok(ativos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Ativo), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var ativo = await _ativoService.GetByIdAsync(id);

            if (ativo == null)
            {
                return NotFound($"Ativo com ID {id} não encontrado.");
            }
            return Ok(ativo);
        }

        // --- CRUD: UPDATE (PUT) ---

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] Ativo ativo)
        {
            if (id != ativo.Id)
            {
                return BadRequest("O ID na rota não corresponde ao ID do corpo da requisição.");
            }

            try
            {
                await _ativoService.UpdateAsync(ativo);
                // Retorna 204 No Content, padrão para PUT/UPDATE bem-sucedido.
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Ativo com ID {id} não encontrado para atualização.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro interno ao processar a atualização do ativo.");
            }
        }

        // --- CRUD: DELETE (DELETE) ---

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _ativoService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                // Captura exceção do Service quando o ativo não é encontrado.
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro interno ao processar a exclusão do ativo.");
            }
        }

        // --- Regras de Negócio e Consultas Específicas ---

        [HttpGet("user/{userId}/categoria/{categoriaId}")]
        [ProducesResponseType(typeof(IEnumerable<Ativo>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetByUserAndCategory(int userId, int categoriaId)
        {
            try
            {
                var ativos = await _ativoService.GetAtivosByCategoriaIdAndUserIdAsync(categoriaId, userId);
                return Ok(ativos);
            }
            catch (ArgumentException ex)
            {
                // Captura validações de IDs (<= 0) do Service
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao buscar ativos pelo usuário e categoria.");
            }
        }

        [HttpGet("disponiveis")]
        [ProducesResponseType(typeof(IEnumerable<Ativo>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDisponiveis()
        {
            var ativos = await _ativoService.GetAtivosDisponiveisParaTransferenciaAsync();
            return Ok(ativos);
        }
    }
}
