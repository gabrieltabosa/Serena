
// FUNÇÃO PRINCIPAL PARA CARREGAR PARTIAL VIEWS (AJAX)

// FUNÇÃO PRINCIPAL PARA CARREGAR PARTIAL VIEWS (AJAX)

function carregarPartial(partialName) {
    // Define o URL do Controller que retorna a Partial View
    // URL ajustada para o HomeController (ex: /Home/LoginPartial)
    var url = '/Home/' + partialName;

    var container = $('#acesso-container');
    // Efeito de carregamento
    container.html('<div class="text-center p-5"><div class="spinner-border text-primary" role="status"><span class="visually-hidden">Carregando...</span></div><p class="mt-2">Carregando...</p></div>');

    // Requisição AJAX
    $.get(url, function (data) {
        container.html(data);

       
        // 2. Configurar eventos, máscaras e validações
        setupPartialEvents();
    }).fail(function () {
        container.html('<div class="alert alert-danger">Erro ao carregar o formulário.</div>');
    });
}


