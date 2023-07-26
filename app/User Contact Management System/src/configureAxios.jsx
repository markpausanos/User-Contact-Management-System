import axios from "axios";
import Cookies from "universal-cookie";
import { UsersService, config } from "./services";

const cookies = new Cookies();

const loginRestart = () => {
	cookies.remove("User", {
		path: "/",
	});
	cookies.remove("AccessToken", {
		path: "/",
	});
};

export const configureAxios = () => {
	axios.defaults.baseURL = config.API_URL;
	axios.defaults.timeout = 40000;
	axios.defaults.headers.common["Content-Type"] = "application/json";

	// add a request interceptor to all the axios requests
	// that are going to be made in the site. The purpose
	// of this interceptor is to verify if the access token
	// is still valid and renew it if needed and possible
	axios.interceptors.request.use(
		(requestConfig) => {
			// if the current request doesn't include the config's base
			// API URL, we don't attach the access token to its authorization
			// because it means it is an API call to a 3rd party service
			if (requestConfig.baseURL !== config.API_URL) {
				return requestConfig;
			}

			// Get access token from cookies for every api request
			const accessToken = cookies.get("AccessToken");
			requestConfig.headers.authorization = accessToken
				? `Bearer ${accessToken}`
				: null;

			return requestConfig;
		},
		(error) => Promise.reject(error)
	);

	axios.interceptors.response.use(null, async (error) => {
		if (error.config && error.response) {
			if (error.response.status === 401) {
				// Get the refreshToken from the cookies
				const refreshToken = cookies.get("RefreshToken");
				const token = cookies.get("AccessToken");
				if (refreshToken) {
					try {
						// Send the refreshToken to the server
						const response = await UsersService.refreshToken({
							Token: token,
							RefreshToken: refreshToken,
						});

						// If the response is successful, update the access token cookie
						if (
							response.status === 200 &&
							response.data &&
							response.data.token
						) {
							// Retry the failed request with the new token
							error.config.headers.authorization = `Bearer ${response.data.token}`;
							return axios(error.config);
						}
					} catch (refreshError) {
						// If refreshing the token fails, restart the login
						loginRestart();
					}
				} else {
					// If there is no refreshToken, restart the login
					loginRestart();
				}
			}
		}

		return Promise.reject(error);
	});
};
