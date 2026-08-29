import { login } from "../services/authService.js";
import { saveToken } from "../auth/authStorage.js";

const loginForm =
    document.getElementById("loginForm");

const loginButton =
    document.getElementById("loginButton");

const errorMessage =
    document.getElementById("errorMessage");


loginForm.addEventListener("submit", async (event) => {

    event.preventDefault();

    errorMessage.textContent = "";
    loginButton.disabled = true;
    loginButton.textContent = "Signing in...";

    const email =
        document.getElementById("email").value.trim();

    const password =
        document.getElementById("password").value;

    try {

        const loginResult =
            await login(email, password);

        saveToken(loginResult.token);

        window.location.href = "/index.html";
    }
    catch (error) {

        errorMessage.textContent = error.message;
    }
    finally {

        loginButton.disabled = false;
        loginButton.textContent = "Sign In";
    }

});