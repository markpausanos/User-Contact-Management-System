import { Button, Typography } from "@mui/material";
import { useState } from "react";
import PropTypes from "prop-types"; // Import the prop-types library
import styles from "./styles.module.scss";
import { ContactsService } from "../../../../services";
const ConfirmDialog = ({
	setOpenPopupDelete,
	setContactsList,
	contactForDelete,
}) => {
	const [isDeleting, setIsDeleting] = useState(false);
	const [errors, setErrors] = useState("");
	return (
		<>
			<div className={styles.ConfirmDialog}>
				<Button
					variant="contained"
					type="submit"
					color="primary"
					disabled={isDeleting}
					onClick={async () => {
						setIsDeleting(true);
						try {
							const deleteResponse = await ContactsService.delete(
								contactForDelete.id
							);
							if (deleteResponse) {
								setContactsList((existingContacts) =>
									existingContacts.filter((x) => x.id !== contactForDelete.id)
								);
							}
						} catch (error) {
							setErrors(error.message || "An error occurred");
						}
						setIsDeleting(false);
						setOpenPopupDelete(false);
					}}
				>
					Yes
				</Button>
				<Button
					variant="contained"
					type="submit"
					color="warning"
					disabled={isDeleting}
					onClick={() => {
						setOpenPopupDelete(false);
					}}
				>
					No
				</Button>
			</div>
			<div className={styles.ConfirmDialog_Text}>
				<Typography margin="10px" color="red" variant="subtitle2">
					{errors && errors}
					{!errors && ""}
				</Typography>
			</div>
		</>
	);
};

// Add prop validation using PropTypes
ConfirmDialog.propTypes = {
	setOpenPopupDelete: PropTypes.func.isRequired,
	setContactsList: PropTypes.func.isRequired,
	contactForDelete: PropTypes.object.isRequired,
};

export default ConfirmDialog;
