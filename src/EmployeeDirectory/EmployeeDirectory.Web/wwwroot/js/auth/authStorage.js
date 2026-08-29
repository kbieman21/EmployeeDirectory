//manage the JWT in browser storage.
const TOKEN_KEY = "employeeDirectory.accessToken";

export function saveToken(token) {
    sessionStorage.setItem(TOKEN_KEY, token);
}

export function getToken() {
    return sessionStorage.getItem(TOKEN_KEY);
}

export function removeToken() {
    sessionStorage.removeItem(TOKEN_KEY);
}

export function hasToken() {
    return getToken() !== null;
}