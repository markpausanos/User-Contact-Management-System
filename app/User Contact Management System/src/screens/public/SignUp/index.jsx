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
import * as yup from "yup";

const passwordValidator =
	/^(?=.*\d)(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z]).{6,}$/;

const fieldRequiredMessage = "This field is required.";
const schema = yup.object().shape({
	email: yup
		.string()
		.email("Please enter a valid email.")
		.required(fieldRequiredMessage),
	firstName: yup
		.string()
		.max(50, "Maximum of 50 characters.")
		.required(fieldRequiredMessage),
	lastName: yup
		.string()
		.max(50, "Maximum of 50 characters.")
		.required(fieldRequiredMessage),
	username: yup
		.string()
		.min(4, "Minimum of 4 characters")
		.max(15, "Maximum of 15 characters.")
		.required(fieldRequiredMessage),
	password: yup
		.string()
		.min(6, "Mininum of 6 characters")
		.matches(passwordValidator, {
			message:
				"Must have at least 1 of the following:\nLowercase letter, Uppercase letter, Special character, Number",
		})
		.required(fieldRequiredMessage),
	confirmPassword: yup
		.string()
		.oneOf([yup.ref("password"), null], "Passwords must match")
		.required(fieldRequiredMessage),
});

const SignUp = () => {
	const userContext = useContext(UserContext);
	const cookies = new Cookies();
	const navigate = useNavigate();
	const [isSigningUp, setIsSigningUp] = useState(false);
	const [showPassword, setShowPassword] = useState(false);

	return (
		<div className={styles.SignUp}>
			<Header />
			<Container className={styles.SignUp_Container}>
				<Paper elevation={10} className={styles.SignUp_Container_Paper}>
					<div className={styles.SignUp_Container_Paper_Grid}>
						<h1>Sign Up for LinkUp</h1>
					</div>
					<hr style={{ width: "50%" }} />

					<Formik
						initialValues={{
							firstName: "",
							lastName: "",
							email: "",
							username: "",
							password: "",
							confirmPassword: "",
						}}
						validationSchema={schema}
						onSubmit={async (values, { setErrors }) => {
							const currentFormValues = {
								firstName: values.firstName,
								lastName: values.lastName,
								email: values.email,
								username: values.username,
								password: values.password,
								confirmPassword: values.confirmPassword,
							};

							setIsSigningUp(true);

							try {
								const { data: registerResponse } = await UsersService.register(
									currentFormValues
								);

								cookies.set("AccessToken", registerResponse.token, {
									path: "/",
									maxAge: 60
								})
								
								const currentTime = new Date();

								// Calculate the expiration date (3 months = 90 days)
								const expirationTime = new Date(currentTime.getTime() + (90 * 24 * 60 * 60 * 1000));

								// Set the cookie with the calculated expiration date
								cookies.set("RefreshToken", registerResponse.refreshToken, {
									path: "/",
									expires: expirationTime,
								});

								const { data: user } = await UsersService.get();

								userContext.loginUpdate(user);

								setErrors({
									overall: registerResponse.error,
								});
							} catch (error) {
								setErrors({
									overall: error.message,
								});
								userContext.loginRestart();
							}

							setIsSigningUp(false);
						}}
					>
						{({ errors, values, touched, handleSubmit, setFieldValue }) => (
							<Form
								className={styles.SignUp_Container_Paper_Form}
								autoComplete="false"
							>
								<div className={styles.SignUp_Container_Paper_Grid}>
									<TextField
										label="First Name"
										className={styles.SignUp_Container_Paper_Textfield}
										name="firstName"
										value={values.firstName}
										error={!isEmpty(errors) && touched.firstName}
										helperText={touched.firstName && errors.firstName}
										required
										fullWidth
										margin="normal"
										size="small"
										onChange={(e) => setFieldValue("firstName", e.target.value)}
									/>
									<TextField
										label="Last Name"
										className={styles.SignUp_Container_Paper_Textfield}
										name="lastname"
										value={values.lastName}
										error={!isEmpty(errors) && touched.lastName}
										helperText={touched.lastName && errors.lastName}
										required
										fullWidth
										margin="normal"
										size="small"
										onChange={(e) => setFieldValue("lastName", e.target.value)}
									/>
									<TextField
										label="Email Address"
										className={styles.SignUp_Container_Paper_Textfield}
										name="email"
										value={values.email}
										error={!isEmpty(errors) && touched.email}
										helperText={touched.email && errors.email}
										required
										fullWidth
										margin="normal"
										size="small"
										onChange={(e) => setFieldValue("email", e.target.value)}
									/>
									<TextField
										label="Username"
										className={styles.SignUp_Container_Paper_Textfield}
										name="username"
										value={values.username}
										error={!isEmpty(errors) && touched.username}
										helperText={touched.username && errors.username}
										required
										fullWidth
										margin="normal"
										size="small"
										onChange={(e) => setFieldValue("username", e.target.value)}
									/>
									<TextField
										label="Password"
										className={styles.SignUp_Container_Paper_Textfield}
										name="password"
										value={values.password}
										error={!isEmpty(errors) && touched.password}
										helperText={touched.password && errors.password}
										required
										fullWidth
										margin="normal"
										type={showPassword ? "text" : "password"}
										size="small"
										onChange={(e) => setFieldValue("password", e.target.value)}
										FormHelperTextProps={{
											sx: {
												width: "100%",
												whiteSpace: "pre-wrap",
											},
										}}
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
									<TextField
										label="Confirm Password"
										className={styles.SignUp_Container_Paper_Textfield}
										name="confirmPassword"
										value={values.confirmPassword}
										error={!isEmpty(errors) && touched.confirmPassword}
										helperText={
											touched.confirmPassword && errors.confirmPassword
										}
										required
										fullWidth
										margin="normal"
										type={showPassword ? "text" : "password"}
										size="small"
										onChange={(e) =>
											setFieldValue("confirmPassword", e.target.value)
										}
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
								<div className={styles.SignUp_Container_Paper_Grid}>
									<Typography margin="10px" color="red" variant="subtitle2">
										{errors && errors.overall}
										{!errors && ""}
									</Typography>
								</div>
								<div className={styles.SignUp_Container_Paper_Grid}>
									<Button
										className={styles.SignUp_Container_Paper_Button}
										variant="contained"
										type="submit"
										color="primary"
										disabled={isSigningUp}
										onClick={handleSubmit}
									>
										{!isSigningUp && "Sign Up"}
										{isSigningUp && <PulseLoader color="white" />}
									</Button>
								</div>
							</Form>
						)}
					</Formik>

					<hr style={{ width: "50%" }} />
					<div>
						<Typography
							variant="body2"
							marginTop={"10px"}
							paddingBottom={"20px"}
						>
							Already have an account?
							<br />
							<span
								className={styles.SignUp_Container_Paper_Typography}
								onClick={() => navigate("/login")}
							>
								Login to LinkUp
							</span>
						</Typography>
					</div>
				</Paper>
			</Container>
		</div>
	);
};

export default SignUp;
