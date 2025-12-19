document.addEventListener("DOMContentLoaded", function () {
    // 1. Captura de elementos
    const formCadastro = document.getElementById('fromCadastro');
    const emailInput = document.querySelector('input[type="email"]');
    const cpfInput = document.querySelector('.cpf-mask');
    const password = document.querySelector('input[name="Password"]');
    const confirmPassword = document.getElementById('ConfirmPassword');

    // Função auxiliar para validar email
    function validarEmail(email) {
        const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return regex.test(email);
    }

    // --- VALIDAÇÃO EM TEMPO REAL ---

    if (emailInput) {
        emailInput.addEventListener('blur', function () {
            this.classList.toggle('is-invalid', !validarEmail(this.value));
        });
    }

    if (cpfInput) {
        cpfInput.addEventListener('blur', function () {
            // Se a função validarCPF não estiver definida neste arquivo, o erro continuará
            if (typeof validarCPF === "function") {
                const valido = validarCPF(this.value);
                this.classList.toggle('is-invalid', !valido);
            }
        });
    }

    if (confirmPassword && password) {
        confirmPassword.addEventListener('input', function () {
            const coincidem = password.value === confirmPassword.value;
            confirmPassword.classList.toggle('is-invalid', !coincidem);
        });
    }

    // --- VALIDAÇÃO NO MOMENTO DO SUBMIT ---

    if (formCadastro) {
        formCadastro.addEventListener('submit', function (event) {
            let formValido = true;

            // 1. Validar Email
            if (emailInput && !validarEmail(emailInput.value)) {
                emailInput.classList.add('is-invalid');
                formValido = false;
            }

            // 2. Validar Confirmação de Senha
            if (password && confirmPassword) {
                if (password.value !== confirmPassword.value) {
                    confirmPassword.classList.add('is-invalid');
                    formValido = false;
                }
            }

            // 3. Verificar se existe qualquer outro campo com a classe is-invalid
            if (document.querySelectorAll('.is-invalid').length > 0) {
                formValido = false;
            }

            if (!formValido) {
                event.preventDefault(); // Para o envio
                event.stopPropagation();
                alert("Por favor, corrija os campos destacados em vermelho.");
            }
        });
    }
}); // Fim do DOMContentLoaded - TUDO deve estar aqui dentro

