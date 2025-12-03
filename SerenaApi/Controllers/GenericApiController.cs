using DominioSerena;
using Microsoft.AspNetCore.Mvc;
using ServiceSerena;
using System.Net;

namespace SerenaApi.Controllers
{


    // Não pode ser instanciada, apenas herdada.
    // ControllerBase é a classe base para Controllers sem suporte a View.
    public abstract class GenericController<T> : ControllerBase where T : class
    {
        // Injeção do Service Genérico (que gerencia o IGenericRepository<T> e o UoW)
        protected readonly IGenericService<T> _service;

        // A classe filha deve passar o service para o construtor base.
        public GenericController(IGenericService<T> service)
        {
            _service = service;
        }

        // --- CRUD: CREATE (POST) ---
        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] T entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _service.AddAsync(entity);
                // Idealmente, retornar CreatedAtAction (requer nome do GetById)
                return StatusCode((int)HttpStatusCode.Created, entity);
            }
            catch (InvalidOperationException ex)
            {
                // Captura regras de negócio básicas (ex: unicidade)
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro interno ao criar a entidade {typeof(T).Name}.");
            }
        }

        // --- CRUD: READ ALL (GET) ---
        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            var entities = await _service.GetAllAsync();
            return Ok(entities);
        }

        // --- CRUD: READ BY ID (GET) ---
        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(int id)
        {
            var entity = await _service.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound($"Entidade {typeof(T).Name} com ID {id} não encontrada.");
            }
            return Ok(entity);
        }

        // --- CRUD: UPDATE (PUT) ---
        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Put(int id, [FromBody] T entity)
        {
            // Nota: Esta comparação de ID genérica pode exigir Reflection
            // ou que T implemente uma interface IEntity com propriedade Id.
            // Simplificando, assumimos que o mapeamento no Service lida com o Id.

            try
            {
                await _service.UpdateAsync(entity);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Entidade {typeof(T).Name} não encontrada para atualização.");
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro interno ao processar a atualização da entidade {typeof(T).Name}.");
            }
        }

        // --- CRUD: DELETE (DELETE) ---
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro interno ao processar a exclusão da entidade {typeof(T).Name}.");
            }
        }
    }
}