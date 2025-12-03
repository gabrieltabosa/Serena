using DominioSerena;
using Microsoft.AspNetCore.Mvc;
using ServiceSerena;
using System.Net;

namespace SerenaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController: ControllerBase
    {

        // Injeção de Dependência da interface do Serviço
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // --- Regra de Negócio: LOGIN / AUTENTICAÇÃO (POST) ---
        // Geralmente um DTO é usado para login, mas simplificaremos para User aqui.
        [HttpPost("login")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Login([FromBody] User userCredentials)
        {
            if (string.IsNullOrWhiteSpace(userCredentials.Email) || string.IsNullOrWhiteSpace(userCredentials.Password))
            {
                return BadRequest(new { Message = "Email e senha são obrigatórios." });
            }

            var user = await _userService.GetUserByCredentialsAsync(
                userCredentials.Email,
                userCredentials.Password);

            if (user == null)
            {
                // Retorna 401 Unauthorized para falha de credenciais
                return Unauthorized(new { Message = "Credenciais inválidas." });
            }

            // Em um sistema real, aqui você geraria e retornaria um JWT (token)
            return Ok(user);
        }

        // --- CRUD: CREATE / REGISTRAR (POST) ---
        [HttpPost("registrar")]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _userService.AddAsync(user);
                // Retorna 201 Created com a rota para buscar o novo recurso.
                return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
            }
            catch (InvalidOperationException ex)
            {
                // Captura regras de negócio violadas (Email ou CPF duplicado)
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro interno ao processar o registro do usuário.");
            }
        }

        // --- CRUD: READ ALL (GET) ---
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Nota: Em um sistema real, essa rota seria protegida por autorização, 
            // pois expõe dados de todos os usuários.
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound($"Usuário com ID {id} não encontrado.");
            }
            return Ok(user);
        }

        // --- CRUD: UPDATE (PUT) ---
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] User user)
        {
            if (id != user.Id)
            {
                return BadRequest("O ID na rota não corresponde ao ID do corpo da requisição.");
            }

            try
            {
                await _userService.UpdateAsync(user);
                return NoContent();
            }
            catch (Exception ex) when (ex is KeyNotFoundException || ex is InvalidOperationException)
            {
                // Captura Not Found ou violações de unicidade
                return StatusCode(ex is KeyNotFoundException ? 404 : 400, new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro interno ao processar a atualização do usuário.");
            }
        }

        // --- CRUD: DELETE (DELETE) ---
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _userService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro interno ao processar a exclusão do usuário.");
            }
        }
    }
}
