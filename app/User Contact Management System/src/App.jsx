import React, { useState } from "react";
import Cookies from "universal-cookie";
import { UserContext } from "./contexts";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { PrivateRoute, NoAuthRoute } from "./hocs";
import { PulseLoader } from "react-spinners";
import { Login, ScreenNoExist } from "./screens/public";
import "./styles/App.scss";
import { createTheme, ThemeProvider } from "@mui/material";

const cookies = new Cookies();
const theme = createTheme({
	typography: {
		fontFamily: "DIN",
	},
});

function App() {
	const [user, setUser] = useState(cookies.get("user"));

	const loginUpdate = (userData) => {
		cookies.set("user", userData, {
			path: "/",
		});

		setUser(userData);
	};

	const loginRestart = () => {
		cookies.remove("user", {
			path: "/",
		});
		cookies.remove("accessToken", {
			path: "/",
		});

		setUser(null);
	};

	return (
		<React.Suspense fallback={<PulseLoader />}>
			<ThemeProvider theme={theme}>
				<UserContext.Provider value={{ user, loginUpdate, loginRestart }}>
					<BrowserRouter>
						<Routes>
							<Route
								exact
								path="/"
								element={<PrivateRoute>{<div>Hello world!</div>}</PrivateRoute>}
							/>
							<Route
								path="/home"
								element={<PrivateRoute element={<div>Hello world!</div>} />}
							/>
							<Route
								path="/login"
								element={
									<NoAuthRoute>
										<Login />
									</NoAuthRoute>
								}
							/>
							<Route path="*" element={<ScreenNoExist />} />
						</Routes>
					</BrowserRouter>
				</UserContext.Provider>
			</ThemeProvider>
		</React.Suspense>
	);
}

export default App;
