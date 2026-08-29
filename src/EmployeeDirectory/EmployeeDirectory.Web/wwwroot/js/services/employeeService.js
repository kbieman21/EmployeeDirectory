import { API_BASE_URL } from "../config/apiConfig.js";
import { apiFetch } from "./apiClient.js";

export async function getEmployees() {

    const response = await apiFetch(
        `${API_BASE_URL}/api/Employee`
    );

    // ...
}