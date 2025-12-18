namespace Serena.Models
{
    public class DashboardViewModel<T> where T : new()
    {
        public DashboardViewType ActiveView { get; set; } = DashboardViewType.Login;

        // Para telas de LISTA (Consulta)
        public IEnumerable<T> Items { get; set; } = new List<T>();

        // Para telas de FORMULÁRIO (Login, Cadastro, Atualização, Exclusão)
        public T CurrentItem { get; set; } = new T();

        public string? SessionId { get; set; }
        public int? UserId { get; set; }

        public string Title { get; set; } = "Gerenciamento";
    }
}
