import PropTypes from "prop-types";
import { Formik, Form } from "formik";
import { useState } from "react";
import * as yup from "yup";
import {
	Button,
	TextField,
	Typography,
	InputAdornment,
	IconButton,
} from "@mui/material";
import { isEmpty } from "lodash";
import styles from "./styles.module.scss";
import { UsersService } from "../../../services";
import { Visibility, VisibilityOff } from "@mui/icons-material";

const passwordValidator =
	/^(?=.*\d)(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z]).{6,}$/;

const fieldRequiredMessage = "This field is required.";
const schema = yup.object().shape({
	oldPassword: yup.string().required(fieldRequiredMessage),
	newPassword: yup
		.string()
		.min(6, "Mininum of 6 characters")
		.matches(passwordValidator, {
			message:
				"Must have at least 1 of the following:\nLowercase letter, Uppercase letter, Special character, Number",
		})

		.notOneOf(
			[yup.ref("oldPassword"), null],
			"New password must not match with old password"
		)
		.required(fieldRequiredMessage),
	confirmPassword: yup
		.string()
		.oneOf([yup.ref("newPassword"), null], "Passwords must match")
		.required(fieldRequiredMessage),
});

const PasswordForm = ({ setOpenPopupPassword }) => {
	const [isUpdatingUser, setIsUpdatingUser] = useState(false);
	const [showPassword, setShowPassword] = useState(false);
	const [showPasswordNew, setShowPasswordNew] = useState(false);

	return (
		<>
			<div>
				<Formik
					initialValues={{
						oldPassword: "",
						newPassword: "",
						confirmPassword: "",
					}}
					validationSchema={schema}
					onSubmit={async (values, { setErrors }) => {
						const currentFormValues = {
							oldPassword: values.oldPassword,
							newPassword: values.newPassword,
							confirmPassword: values.confirmPassword,
						};

						setIsUpdatingUser(true);

						try {
							const { data: updateResponse } =
								await UsersService.updatePassword(currentFormValues);

							if (updateResponse) {
								setOpenPopupPassword(false);
								setIsUpdatingUser(false);
							}
						} catch (error) {
							setErrors({
								// Fix: Wrap the error message in an object
								overall: error.message,
							});
							setIsUpdatingUser(false);
						}
					}}
				>
					{({ errors, values, touched, handleSubmit, setFieldValue }) => (
						<Form className={styles.PasswordForm_Form}>
							<div>
								<TextField
									label="Old Password"
									name="oldPassword"
									value={values.oldPassword}
									error={!isEmpty(errors) && touched.oldPassword}
									helperText={errors.oldPassword}
									required
									fullWidth
									margin="normal"
									type={showPassword ? "text" : "password"}
									onChange={(e) => setFieldValue("oldPassword", e.target.value)}
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
									className={styles.PasswordForm_Form_Textfield}
									label="New Password"
									name="newPassword"
									value={values.newPassword}
									error={!isEmpty(errors) && touched.newPassword}
									helperText={errors.newPassword}
									required
									fullWidth
									margin="normal"
									type={showPasswordNew ? "text" : "password"}
									onChange={(e) => setFieldValue("newPassword", e.target.value)}
									InputProps={{
										endAdornment: (
											<InputAdornment position="end">
												<IconButton
													aria-label="toggle password visibility"
													onClick={() => setShowPasswordNew((show) => !show)}
													onMouseDown={(e) => e.preventDefault()}
													edge="end"
												>
													{showPasswordNew ? <Visibility /> : <VisibilityOff />}
												</IconButton>
											</InputAdornment>
										),
									}}
								/>
								<TextField
									className={styles.PasswordForm_Form_Textfield}
									label="Confirm Password"
									name="confirmPassword"
									value={values.confirmPassword}
									error={!isEmpty(errors) && touched.confirmPassword}
									helperText={errors.confirmPassword}
									required
									fullWidth
									margin="normal"
									type={showPasswordNew ? "text" : "password"}
									onChange={(e) =>
										setFieldValue("confirmPassword", e.target.value)
									}
									InputProps={{
										endAdornment: (
											<InputAdornment position="end">
												<IconButton
													aria-label="toggle password visibility"
													onClick={() => setShowPasswordNew((show) => !show)}
													onMouseDown={(e) => e.preventDefault()}
													edge="end"
												>
													{showPasswordNew ? <Visibility /> : <VisibilityOff />}
												</IconButton>
											</InputAdornment>
										),
									}}
								/>
							</div>
							<hr style={{ width: "100%" }} />
							<div>
								<Typography margin="10px" color="red" variant="subtitle2">
									{!isEmpty(errors) && "Invalid password"}
								</Typography>
							</div>
							<div className={styles.PasswordForm_Form_Buttons}>
								<Button
									variant="contained"
									type="submit"
									color="secondary"
									disabled={isUpdatingUser}
									onClick={() => {
										setFieldValue("oldPassword", "");
										setFieldValue("newPassword", "");
										setFieldValue("confirmPassword", "");
									}}
								>
									Reset
								</Button>
								<Button
									variant="contained"
									type="submit"
									color="primary"
									disabled={isUpdatingUser}
									onClick={handleSubmit}
								>
									Update Password
								</Button>
							</div>
						</Form>
					)}
				</Formik>
			</div>
		</>
	);
};

PasswordForm.propTypes = {
	setOpenPopupPassword: PropTypes.func.isRequired,
};

export default PasswordForm;
