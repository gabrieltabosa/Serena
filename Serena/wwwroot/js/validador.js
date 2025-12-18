document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById('fromCadastro');

    // Seleção dos campos
    const emailInput = document.querySelector('input[name="Email"]');
    const password = document.querySelector('input[name="Password"]');
    const confirmPassword = document.getElementById('ConfirmPassword');
    const confirmError = document.getElementById('confirmPasswordError');

    // Função de validação de E-mail via Regex
    function validarEmail(email) {
        const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return regex.test(email);
    }

    // Validação em tempo real para o E-mail
    emailInput.addEventListener('input', function () {
        if (validarEmail(this.value)) {
            this.classList.remove('is-invalid');
            this.classList.add('is-valid');
        } else {
            this.classList.remove('is-valid');
            this.classList.add('is-invalid');
        }
    });

    // Validação em tempo real para Senhas
    confirmPassword.addEventListener('input', function () {
        if (password.value !== confirmPassword.value) {
            confirmPassword.classList.add('is-invalid');
            confirmError.classList.remove('d-none');
        } else {
            confirmPassword.classList.remove('is-invalid');
            confirmPassword.classList.add('is-valid');
            confirmError.classList.add('d-none');
        }
    });

    // Interceptar o Submit
    form.addEventListener('submit', function (event) {
        let formValido = true;

        // Validar E-mail no submit
        if (!validarEmail(emailInput.value)) {
            emailInput.classList.add('is-invalid');
            formValido = false;
        }

        // Validar Confirmação de Senha no submit
        if (password.value !== confirmPassword.value) {
            confirmPassword.classList.add('is-invalid');
            formValido = false;
        }

        if (!formValido) {
            event.preventDefault();
            event.stopPropagation();
            alert("Por favor, corrija os erros no formulário antes de continuar.");
        }
    });
});


