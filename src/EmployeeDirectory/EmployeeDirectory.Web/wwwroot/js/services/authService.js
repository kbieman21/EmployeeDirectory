import { API_BASE_URL } from "../config/apiConfig.js";

export async function login(email, password) {

    const response = await fetch(
        `${API_BASE_URL}/api/Auth/login`,
        {
            method: "POST",

            headers: {
                "Content-Type": "application/json"
            },

            body: JSON.stringify({
                email,
                password
            })
        }
    );

    if (!response.ok) {

        if (response.status === 401) {
            throw new Error("Invalid email or password.");
        }

        throw new Error(
            `Login failed with status ${response.status}.`
        );
    }

    return await response.json();
}