import { isLocal } from "../utils/destinations";

let apiUrl = "https://ucms-api.azurewebsites.net/api";

if (isLocal) {
	apiUrl = "https://localhost:7294/api";
}

const config = {
	API_URL: apiUrl,
};

export default config;
