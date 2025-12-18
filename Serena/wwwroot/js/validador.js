document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("fromCadastro");
    const password = document.getElementById("Password");
    const confirmPassword = document.getElementById("ConfirmPassword");
    const confirmError = document.getElementById("confirmPasswordError");

    // --- 1. Máscaras de Entrada ---
    const applyMask = (input, maskFunc) => {
        input.addEventListener("input", (e) => {
            e.target.value = maskFunc(e.target.value);
            validateField(e.target); // Valida esteticamente ao digitar
        });
    };

    const cpfMask = (v) => {
        v = v.replace(/\D/g, "");
        v = v.replace(/(\d{3})(\d)/, "$1.$2");
        v = v.replace(/(\d{3})(\d)/, "$1.$2");
        v = v.replace(/(\d{3})(\d{1,2})$/, "$1-$2");
        return v.substring(0, 14);
    };

    const telMask = (v) => {
        v = v.replace(/\D/g, "");
        v = v.replace(/^(\d{2})(\d)/g, "($1) $2");
        v = v.replace(/(\d{5})(\d)/, "$1-$2");
        return v.substring(0, 15);
    };

    const cpfInput = document.querySelector(".cpf-mask");
    const telInput = document.querySelector(".cel-mask");
    if (cpfInput) applyMask(cpfInput, cpfMask);
    if (telInput) applyMask(telInput, telMask);

    // --- 2. Validação Estética (Campos Bonitos) ---
    const validateField = (input) => {
        // Se o campo for preenchido corretamente (baseado no tipo do input)
        if (input.checkValidity() && input.value.trim() !== "") {
            input.classList.remove("is-invalid");
            input.classList.add("is-valid");
        } else {
            input.classList.remove("is-valid");
            input.classList.add("is-invalid");
        }
    };

    // Aplica validação visual em todos os inputs ao perder o foco
    form.querySelectorAll("input").forEach(input => {
        input.addEventListener("blur", () => validateField(input));
    });

    // --- 3. Validação de Senha Específica ---
    function validatePasswords() {
        const passValue = password.value;
        const confirmValue = confirmPassword.value;

        if (confirmValue === "") {
            confirmError.classList.add("d-none");
            confirmPassword.classList.remove("is-invalid", "is-valid");
            return false;
        }

        if (passValue === confirmValue) {
            confirmError.classList.add("d-none");
            confirmPassword.classList.remove("is-invalid");
            confirmPassword.classList.add("is-valid");
            return true;
        } else {
            confirmError.classList.remove("d-none");
            confirmPassword.classList.remove("is-valid");
            confirmPassword.classList.add("is-invalid");
            return false;
        }
    }

    password.addEventListener("input", validatePasswords);
    confirmPassword.addEventListener("input", validatePasswords);

    // --- 4. Bloqueio de Envio se houver erros ---
    form.addEventListener("submit", function (event) {
        const isPasswordMatch = validatePasswords();
        const isFormValid = form.checkValidity();

        if (!isFormValid || !isPasswordMatch) {
            event.preventDefault();
            event.stopPropagation();

            // Destaca todos os campos inválidos para o usuário ver
            form.querySelectorAll("input").forEach(validateField);

            alert("Por favor, verifique os campos destacados em vermelho.");
        }
    });
});


