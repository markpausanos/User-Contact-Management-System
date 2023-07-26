import { isLocal } from "../utils/destinations";

let apiUrl = "https://trailblazers-starrail.azurewebsites.net";

if (isLocal) {
	apiUrl = "https://localhost:7294/api";
}

const config = {
	API_URL: apiUrl,
};

export default config;
