import { Auth0Client } from "@auth0/auth0-spa-js";
import { env } from '$env/dynamic/public';
import { writable } from "svelte/store";

let auth0Client: Auth0Client;

export const isAuthenticated = writable(false);
export const user = writable<any>(null);
export const isLoading = writable(true);

export async function initAuth0() {
    auth0Client = new Auth0Client({
        domain: env.PUBLIC_DOMAIN as string,
        clientId: env.PUBLIC_CLIENT_ID as string,
        authorizationParams: {
            redirect_uri: window.location.origin,
            audience: env.PUBLIC_API_URL as string
        },
        cacheLocation: 'localstorage',
        useRefreshTokens: true
    });

    try {
        if (window.location.search.includes('code=') && window.location.search.includes('state=')) {
            await auth0Client.handleRedirectCallback();
            window.history.replaceState({}, document.title, window.location.pathname);
        }

        const authenticated = await auth0Client.isAuthenticated();
        isAuthenticated.set(authenticated);

        if (authenticated) {
            const userProfile = await auth0Client.getUser();
            user.set(userProfile);
        }
    } catch (error) {
        console.error('Auth0 init error:', error);
    } finally {
        isLoading.set(false);
    }
}

export async function login() {
    if (!auth0Client) return;
    await auth0Client.loginWithRedirect();
}

export async function logout() {
    if (!auth0Client) return;

    isAuthenticated.set(false);
    user.set(null);

    auth0Client.logout(); // pravi logout jer loklano svakako ne radi...
}

export async function getAccessToken() {
    if (!auth0Client) return null;
    try {
        return await auth0Client.getTokenSilently();
    } catch (error) {
        console.error('Error getting access token:', error);
        return null;
    }
}