import PropTypes from "prop-types";
import { Formik, Form } from "formik";
import { useState } from "react";
import * as yup from "yup";
import { Button, TextField, Typography } from "@mui/material";
import { isEmpty } from "lodash";
import styles from "./styles.module.scss";
import { UsersService } from "../../../services";

const fieldRequiredMessage = "This field is required.";
const schema = yup.object().shape({
	firstName: yup
		.string()
		.max(50, "Maximum of 50 characters.")
		.required(fieldRequiredMessage),
	lastName: yup
		.string()
		.max(50, "Maximum of 50 characters.")
		.required(fieldRequiredMessage),
});

const UserForm = ({ setOpenPopupUser, setUser, userForEdit }) => {
	const [isUpdatingUser, setIsUpdatingUser] = useState(false);

	return (
		<>
			<div>
				<Formik
					initialValues={{
						firstName: userForEdit?.firstName || "",
						lastName: userForEdit?.lastName || "",
					}}
					validationSchema={schema}
					onSubmit={async (values, { setErrors }) => {
						const currentFormValues = {
							firstName: values.firstName,
							lastName: values.lastName,
						};

						setIsUpdatingUser(true);

						try {
							if (userForEdit) {
								const { data: updateResponse } =
									await UsersService.updateDetails(currentFormValues);

								if (updateResponse) {
									setUser(currentFormValues);
									setOpenPopupUser(false);
									setIsUpdatingUser(false);
								}
							} else {
								throw new Error("Cannot update contact");
							}
						} catch (error) {
							setErrors({
								// Fix: Wrap the error message in an object
								firstName: error.message,
								lastName: error.message,
							});
							setIsUpdatingUser(false);
						}
					}}
				>
					{({ errors, values, touched, handleSubmit, setFieldValue }) => (
						<Form className={styles.UserForm_Form}>
							<div>
								<TextField
									label="First Name"
									name="firstName"
									value={values.firstName}
									error={!isEmpty(errors) && touched.firstName}
									helperText={errors.firstName}
									required
									fullWidth
									margin="normal"
									onChange={(e) => setFieldValue("firstName", e.target.value)}
								/>
								<TextField
									className={styles.UserForm_Form_Textfield}
									label="Last Name"
									name="lastName"
									value={values.lastName}
									error={!isEmpty(errors) && touched.lastName}
									helperText={errors.lastName}
									required
									fullWidth
									margin="normal"
									onChange={(e) => setFieldValue("lastName", e.target.value)}
								/>
							</div>
							<hr style={{ width: "100%" }} />
							<div>
								<Typography margin="10px" color="red" variant="subtitle2">
									{!isEmpty(errors) && "Something went wrong"}
								</Typography>
							</div>
							<div className={styles.UserForm_Form_Buttons}>
								<Button
									variant="contained"
									type="submit"
									color="secondary"
									disabled={isUpdatingUser}
									onClick={() => {
										setFieldValue("firstName", "");
										setFieldValue("lastName", "");
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
									Update User Details
								</Button>
							</div>
						</Form>
					)}
				</Formik>
			</div>
		</>
	);
};

UserForm.propTypes = {
	setOpenPopupUser: PropTypes.func.isRequired,
	setUser: PropTypes.func.isRequired,
	userForEdit: PropTypes.object,
};

export default UserForm;
