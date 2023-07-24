import React, { useState } from "react";
import Cookies from "universal-cookie";
import { UserContext } from "./contexts";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { PrivateRoute, NoAuthRoute } from "./hocs";
import { PulseLoader } from "react-spinners";
import { Login, ForgotPassword, ScreenNoExist, SignUp } from "./screens/public";
import "./styles/App.scss";
import { createTheme, ThemeProvider } from "@mui/material";
import Home from "./screens/public/Home";

const cookies = new Cookies();
const theme = createTheme({
	typography: {
		fontFamily: "DIN",
	},
});

function App() {
	const [user, setUser] = useState(cookies.get("User"));

	const loginUpdate = (userData) => {
		cookies.set("User", userData, {
			path: "/",
		});

		console.log(`hello ${userData}`);
		setUser(userData);
	};

	const loginRestart = () => {
		cookies.remove("User", {
			path: "/",
		});
		cookies.remove("AccessToken", {
			path: "/",
		});
		cookies.remove("RefreshToken", {
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
							<Route exact path="/" element={<Navigate to="/home" replace />} />
							<Route
								path="/home"
								element={
									<PrivateRoute>
										<Home />
									</PrivateRoute>
								}
							/>
							<Route
								path="/login"
								element={
									<NoAuthRoute>
										<Login />
									</NoAuthRoute>
								}
							/>
							<Route
								path="/sign-up"
								element={
									<NoAuthRoute>
										<SignUp />
									</NoAuthRoute>
								}
							/>
							<Route
								path="/forgot-password"
								element={
									<NoAuthRoute>
										<ForgotPassword />
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
