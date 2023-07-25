import PropTypes from "prop-types";
import { Formik, Form } from "formik";
import { useState } from "react";
import * as yup from "yup";
import { Button, TextField, Typography } from "@mui/material";
import { isEmpty } from "lodash";
import styles from "./styles.module.scss";
import { ContactsService } from "../../../../services";

const fieldRequiredMessage = "This field is required.";
const schema = yup.object().shape({
	emailAddress: yup
		.string()
		.email("Please enter a valid email address.")
		.required(fieldRequiredMessage),
	firstName: yup
		.string()
		.max(50, "Maximum of 50 characters.")
		.required(fieldRequiredMessage),
	lastName: yup
		.string()
		.max(50, "Maximum of 50 characters.")
		.required(fieldRequiredMessage),
	contactNumber: yup.string().max(15, "Maximum of 15 characters."),
});

const ContactForm = ({ setOpenPopup, setContactsList, contactForEdit }) => {
	const [isCreatingContact, setIsCreatingContact] = useState(false);

	return (
		<>
			<Formik
				initialValues={{
					id: contactForEdit?.id,
					emailAddress: contactForEdit?.emailAddress || "",
					firstName: contactForEdit?.firstName || "",
					lastName: contactForEdit?.lastName || "",
					contactNumber: contactForEdit?.contactNumber || "",
					deliveryAddress: contactForEdit?.deliveryAddress || "",
					billingAddress: contactForEdit?.billingAddress || "",
				}}
				validationSchema={schema}
				onSubmit={async (values, { setErrors }) => {
					const currentFormValues = {
						id: values.id,
						emailAddress: values.emailAddress,
						firstName: values.firstName,
						lastName: values.lastName,
						contactNumber: values.contactNumber,
						deliveryAddress: values.deliveryAddress,
						billingAddress: values.billingAddress,
					};

					setIsCreatingContact(true);

					try {
						if (contactForEdit != null) {
							const { data: updateResponse } = await ContactsService.update(
								contactForEdit.id,
								currentFormValues
							);
							if (updateResponse) {
								setContactsList((existingContacts) => {
									return existingContacts.map((item) => {
										if (item.id == currentFormValues.id) {
											return currentFormValues;
										}

										return item;
									});
								});
							} else {
								throw "Cannot update contact";
							}
						} else {
							const { data: createResponse } = await ContactsService.create(
								currentFormValues
							);
							if (createResponse) {
								setContactsList((existingContacts) => {
									if (!existingContacts) {
										return [createResponse];
									}

									return [...existingContacts, createResponse];
								});
							}
						}

						setOpenPopup(false);
					} catch (error) {
						setErrors({
							overall: error.message,
						});
					}

					setIsCreatingContact(false);
				}}
			>
				{({ errors, values, touched, handleSubmit, setFieldValue }) => (
					<Form className={styles.ContactForm_Form}>
						<div>
							<TextField
								label="Email"
								name="emailAddress"
								value={values.emailAddress}
								error={!isEmpty(errors) && touched.emailAddress}
								helperText={errors.emailAddress}
								required
								fullWidth
								margin="normal"
								onChange={(e) => setFieldValue("emailAddress", e.target.value)}
							/>
							<TextField
								className={styles.ContactForm_Form_Textfield}
								label="First Name"
								name="firstName"
								value={values.firstName}
								error={!isEmpty(errors) && touched.firstName}
								helperText={errors.firstName}
								required
								margin="normal"
								onChange={(e) => setFieldValue("firstName", e.target.value)}
							/>
							<TextField
								className={styles.ContactForm_Form_Textfield}
								label="Last Name"
								name="lastName"
								value={values.lastName}
								error={!isEmpty(errors) && touched.lastName}
								helperText={errors.lastName}
								required
								margin="normal"
								onChange={(e) => setFieldValue("lastName", e.target.value)}
							/>
							<TextField
								className={styles.ContactForm_Form_Textfield}
								label="Contact Number"
								name="contactNumber"
								value={values.contactNumber}
								margin="normal"
								onChange={(e) => setFieldValue("contactNumber", e.target.value)}
							/>
							<TextField
								label="Delivery Address"
								name="deliveryAddress"
								value={values.deliveryAddress}
								margin="normal"
								fullWidth
								multiline
								onChange={(e) =>
									setFieldValue("deliveryAddress", e.target.value)
								}
							/>
							<TextField
								label="Billing Address"
								name="billingAddress"
								value={values.billingAddress}
								margin="normal"
								fullWidth
								multiline
								onChange={(e) =>
									setFieldValue("billingAddress", e.target.value)
								}
							/>
						</div>
						<hr style={{ width: "100%" }} />
						<div>
							<Typography margin="10px" color="red" variant="subtitle2">
								{errors && errors.overall}
								{!errors && ""}
							</Typography>
						</div>
						<div className={styles.ContactForm_Form_Buttons}>
							<Button
								variant="contained"
								type="submit"
								color="secondary"
								disabled={isCreatingContact}
								onClick={() => {
									setFieldValue("emailAddress", "");
									setFieldValue("firstName", "");
									setFieldValue("lastName", "");
									setFieldValue("contactNumber", "");
									setFieldValue("deliveryAddress", "");
									setFieldValue("billingAddress", "");
								}}
							>
								Reset
							</Button>
							<Button
								variant="contained"
								type="submit"
								color="primary"
								disabled={isCreatingContact}
								onClick={handleSubmit}
							>
								{contactForEdit && "Update Contact"}
								{!contactForEdit && "Add Contact"}
							</Button>
						</div>
					</Form>
				)}
			</Formik>
		</>
	);
};

ContactForm.propTypes = {
	setOpenPopup: PropTypes.func.isRequired,
	setContactsList: PropTypes.func.isRequired,
	contactForEdit: PropTypes.object,
};

export default ContactForm;
