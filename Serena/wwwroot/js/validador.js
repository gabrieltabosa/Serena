
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


function setupPartialEvents() {

    // --- Lógica de Troca de Tela ---

    // Troca para o Cadastro
    $('#link-para-cadastro').off('click').on('click', function (e) {
        e.preventDefault();
        carregarPartial('CadastroPartial');
    });

    // Troca para o Login
    $('#link-para-login').off('click').on('click', function (e) {
        e.preventDefault();
        carregarPartial('LoginPartial');
    });

    // Máscara para CPF (Requer jQuery Mask Plugin)
    if (typeof $.fn.mask === 'function') { // Checa se a biblioteca está carregada
        $('#inputCPFCadastro').mask('000.000.000-00');

        // Máscara para Telefone/Celular
        var maskBehavior = function (val) {
            return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
        };
        $('#inputTelefone').mask(maskBehavior, {
            onKeyPress: function (val, e, field, options) {
                field.mask(maskBehavior.apply({}, arguments), options);
            }
        });
    }

    // Exemplo de validação customizada para Senha (HTML5)
    $('#inputSenhaCadastro').attr('minlength', '8');
    $('#inputConfirmarSenha').attr('minlength', '8');

   
}


$(document).ready(function () {
    // Inicia carregando a tela de Login
    carregarPartial('LoginPartial');
});