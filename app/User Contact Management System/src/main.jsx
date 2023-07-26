import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.jsx";

// Reset
import "./styles/normalize.scss";

// Configurations
import { configureAxios } from "./configureAxios";

configureAxios();

ReactDOM.createRoot(document.getElementById("root")).render(
	<React.StrictMode>
		<App />
	</React.StrictMode>
);
