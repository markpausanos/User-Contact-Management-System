import { useState, useContext } from "react";
import { useNavigate } from "react-router-dom";
import { Form, Formik } from "formik";
import { isEmpty } from "lodash";
import { UserContext } from "../../../contexts";
import { UsersService } from "../../../services";
import styles from "./styles.module.scss";
import { Header } from "../../../components";
import { Container, Paper, TextField, Button, Typography } from "@mui/material";
import { PulseLoader } from "react-spinners";

const validate = (values) => {
	const errors = {};

	if (!values.email) {
		errors.email = "This field is required.";
	}

	return errors;
};

const ForgotPassword = () => {
	const userContext = useContext(UserContext);
	const navigate = useNavigate();
	const [isSending, setIsSending] = useState(false);

	return (
		<div className={styles.ForgotPassword}>
			<Header />
			<Container className={styles.ForgotPassword_Container}>
				<Paper elevation={10} className={styles.ForgotPassword_Container_Paper}>
					<div className={styles.ForgotPassword_Container_Paper_Grid}>
						<h1>Forgot Password</h1>
					</div>
					<hr style={{ width: "50%" }} />

					<Formik
						initialValues={{
							email: "",
						}}
						onSubmit={async (values, { setErrors }) => {
							const currentFormValues = {
								email: values.username,
							};

							const errors = validate(values);

							if (!isEmpty(errors)) {
								setErrors(errors);
								return;
							}

							setIsSending(true);

							try {
								await UsersService.ForgotPassword(currentFormValues);

								userContext.ForgotPasswordUpdate({
									username: currentFormValues.username,
								});
								setErrors(null);
							} catch (error) {
								setErrors({
									overall: "Invalid username and/or password.",
								});
							}

							setIsSending(false);
						}}
					>
						{({ errors, values, handleSubmit, setFieldValue }) => (
							<Form className={styles.ForgotPassword_Container_Paper_Form}>
								<div className={styles.ForgotPassword_Container_Paper_Grid}>
									<TextField
										label="Enter email"
										className={styles.ForgotPassword_Container_Paper_Textfield}
										name="email"
										value={values.email}
										error={!isEmpty(errors)}
										helperText={errors.email}
										required
										fullWidth
										margin="normal"
										size="small"
										onChange={(e) => setFieldValue("email", e.target.value)}
									/>
								</div>
								<div className={styles.ForgotPassword_Container_Paper_Grid}>
									<Typography margin="10px" color="green" variant="subtitle2">
										{!errors && (
											<>
												Please wait for the team to look on this request.
												<br />
												New password will be sent through email.
											</>
										)}
									</Typography>
								</div>
								<div className={styles.ForgotPassword_Container_Paper_Grid}>
									<Button
										className={styles.ForgotPassword_Container_Paper_Button}
										variant="contained"
										type="submit"
										color="primary"
										disabled={isSending}
										onClick={handleSubmit}
										sx={{ marginBottom: "1vh" }}
									>
										{!isSending && "Send Reset Request"}
										{isSending && <PulseLoader color="white" />}
									</Button>
								</div>
							</Form>
						)}
					</Formik>

					<hr style={{ width: "50%" }} />
					<div>
						<Typography variant="body2" paddingBottom={"20px"}>
							<br />
							<span
								className={styles.ForgotPassword_Container_Paper_Typography}
								onClick={() => navigate("/login")}
							>
								Back to Login
							</span>
						</Typography>
					</div>
				</Paper>
			</Container>
		</div>
	);
};

export default ForgotPassword;
