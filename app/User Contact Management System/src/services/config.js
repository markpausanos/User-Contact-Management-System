import { isLocal } from "../utils/destinations";

let apiUrl = "https://ucms-api.azurewebsites.net/api";

if (isLocal) {
	apiUrl = "http://localhost:8080/api";
}

const config = {
	API_URL: apiUrl,
};

export default config;
