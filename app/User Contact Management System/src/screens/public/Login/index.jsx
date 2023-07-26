import { useState, useContext } from "react";
import Cookies from "universal-cookie";
import { useNavigate } from "react-router-dom";
import { Form, Formik } from "formik";
import { isEmpty } from "lodash";
import { UserContext } from "../../../contexts";
import { UsersService } from "../../../services";
import styles from "./styles.module.scss";
import { Header } from "../../../components";
import {
	Container,
	Paper,
	TextField,
	Button,
	Typography,
	InputAdornment,
	IconButton,
} from "@mui/material";
import { PulseLoader } from "react-spinners";
import { Visibility, VisibilityOff } from "@mui/icons-material";

const validate = (values) => {
	const errors = {};

	if (!values.username) {
		errors.username = "This field is required.";
	}

	if (!values.password) {
		errors.password = "This field is required.";
	}

	return errors;
};

const Login = () => {
	const userContext = useContext(UserContext);
	const cookies = new Cookies();
	const navigate = useNavigate();
	const [isLoggingIn, setIsLoggingIn] = useState(false);
	const [showPassword, setShowPassword] = useState(false);

	return (
		<div className={styles.Login}>
			<Header />
			<Container className={styles.Login_Container}>
				<Paper elevation={10} className={styles.Login_Container_Paper}>
					<div className={styles.Login_Container_Paper_Grid}>
						<h1>Login to LinkUp</h1>
					</div>
					<hr style={{ width: "50%" }} />

					<Formik
						initialValues={{
							username: "",
							password: "",
						}}
						onSubmit={async (values, { setErrors }) => {
							const currentFormValues = {
								username: values.username,
								password: values.password,
							};

							const errors = validate(values);

							if (!isEmpty(errors)) {
								setErrors(errors);
								return;
							}

							setIsLoggingIn(true);

							try {
								const { data: loginResponse} = await UsersService.login(currentFormValues);
								
								cookies.set("AccessToken", loginResponse, {
									path: "/",
									maxAge: 60
								})
								
								const currentTime = new Date();

								// Calculate the expiration date (3 months = 90 days)
								const expirationTime = new Date(currentTime.getTime() + (90 * 24 * 60 * 60 * 1000));

								// Set the cookie with the calculated expiration date
								cookies.set("RefreshToken", loginResponse, {
									path: "/",
									expires: expirationTime,
								});

								const { data: user } = await UsersService.get();

								await userContext.loginUpdate(user);

								setErrors(null);
							} catch (error) {
								setErrors({
									overall: "Invalid username and/or password.",
								});
								userContext.loginRestart();
							}

							setIsLoggingIn(false);
						}}
					>
						{({ errors, values, handleSubmit, setFieldValue }) => (
							<Form className={styles.Login_Container_Paper_Form}>
								<div className={styles.Login_Container_Paper_Grid}>
									<TextField
										label="Username"
										className={styles.Login_Container_Paper_Textfield}
										name="username"
										value={values.username}
										error={!isEmpty(errors)}
										helperText={errors.username}
										required
										fullWidth
										margin="normal"
										size="small"
										onChange={(e) => setFieldValue("username", e.target.value)}
									/>
									<TextField
										label="Password"
										className={styles.Login_Container_Paper_Textfield}
										name="password"
										value={values.password}
										error={!isEmpty(errors)}
										helperText={errors.password}
										required
										fullWidth
										margin="normal"
										type={showPassword ? "text" : "password"}
										size="small"
										onChange={(e) => setFieldValue("password", e.target.value)}
										InputProps={{
											endAdornment: (
												<InputAdornment position="end">
													<IconButton
														aria-label="toggle password visibility"
														onClick={() => setShowPassword((show) => !show)}
														onMouseDown={(e) => e.preventDefault()}
														edge="end"
													>
														{showPassword ? <Visibility /> : <VisibilityOff />}
													</IconButton>
												</InputAdornment>
											),
										}}
									/>
								</div>
								<div className={styles.Login_Container_Paper_Grid}>
									<Typography margin="10px" color="red" variant="subtitle2">
										{errors && errors.overall}
										{!errors && ""}
									</Typography>
								</div>
								<div className={styles.Login_Container_Paper_Grid}>
									<Button
										className={styles.Login_Container_Paper_Button}
										variant="contained"
										type="submit"
										color="primary"
										disabled={isLoggingIn}
										onClick={handleSubmit}
									>
										{!isLoggingIn && "Sign In"}
										{isLoggingIn && <PulseLoader color="white" />}
									</Button>
								</div>
							</Form>
						)}
					</Formik>

					<div>
						<Typography
							className={styles.Login_Container_Paper_Typography}
							variant="body2"
							margin={"20px 0px 10px 0px"}
							onClick={() => navigate("/forgot-password")}
						>
							{/* Forgot your password? */}
						</Typography>
					</div>
					<hr style={{ width: "50%" }} />
					<div>
						<Typography
							variant="body2"
							marginTop={"10px"}
							paddingBottom={"20px"}
						>
							Don{`'`}t have an account?
							<br />
							<span
								className={styles.Login_Container_Paper_Typography}
								onClick={() => navigate("/sign-up")}
							>
								Sign Up for LinkUp
							</span>
						</Typography>
					</div>
				</Paper>
			</Container>
		</div>
	);
};

export default Login;
