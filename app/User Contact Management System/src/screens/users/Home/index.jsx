import { useState, useEffect } from "react";
import { PulseLoader } from "react-spinners";
import { NavHeader, Popup } from "../../../components";
import { useContacts, useTable } from "../../../hooks";
import styles from "./styles.module.scss";
import {
	Paper,
	TableBody,
	TableRow,
	TableCell,
	Toolbar,
	TextField,
	InputAdornment,
	Typography,
	Icon,
	Button,
} from "@mui/material";
import {
	Add,
	Search,
	Contacts,
	EditOutlined,
	CloseOutlined,
} from "@mui/icons-material";
import ContactForm from "./ContactForm";
import ConfirmDialog from "./ConfirmDialog";

const headCells = [
	{
		id: "email",
		label: "Email Address",
	},
	{
		id: "firstName",
		label: "First Name",
	},
	{
		id: "lastName",
		label: "Last Name",
	},
	{
		id: "number",
		label: "Contact Number",
	},
	{
		id: "billing",
		label: "Billing Address",
	},
	{
		id: "delivery",
		label: "Delivery Address",
	},
	{
		id: "action",
		label: "Actions",
	},
];

const Home = () => {
	const {
		contacts: contacts,
		isLoading: isLoading,
		refreshContacts: refreshContacts,
	} = useContacts();

	const [contactsList, setContactsList] = useState([...contacts]);
	const [selectedContact, setSelectedContact] = useState(null);
	const [openPopup, setOpenPopup] = useState(false);
	const [openPopupDelete, setOpenPopupDelete] = useState(false);

	const [filterFn, setFilterFn] = useState({
		fn: (items) => {
			return items;
		},
	});

	useEffect(() => {
		refreshContacts();
	}, []);

	useEffect(() => {
		setContactsList(contacts);
	}, [contacts]);

	const {
		TblContainer,
		TblHead,
		TblPagination,
		contactsAfterPagingAndSorting,
	} = useTable({
		contacts: contactsList ? contactsList : [],
		headCells,
		filterFn,
	});

	const handleSearch = (e) => {
		let target = e.target;
		setFilterFn({
			fn: (items) => {
				if (target.value === "") return items;
				else {
					const targetValue = target.value.trim().toLowerCase();

					return items.filter(
						(x) =>
							x.firstName?.trim().toLowerCase().includes(targetValue) ||
							x.lastName?.trim().toLowerCase().includes(targetValue) ||
							x.email?.trim().toLowerCase().includes(targetValue) ||
							x.billingAddress?.trim().toLowerCase().includes(targetValue) ||
							x.deliveryAddress?.trim().toLowerCase().includes(targetValue) ||
							x.contactNumber?.trim().toLowerCase().includes(targetValue)
					);
				}
			},
		});
	};

	return (
		<>
			<div>
				<NavHeader />
			</div>

			<div className={styles.Home}>
				<>
					<Paper elevation={10} className={styles.Home_Paper}>
						<div className={styles.Home_TableTitle_Header}>
							<div className={styles.Home_TableTitle_Title}>
								<Icon className={styles.Home_TableTitle_Title_Icon}>
									<Contacts
										className={styles.Home_TableTitle_Title_Icon_IconContact}
										sx={{
											display: "flex",
											fontSize: "1.5vw",
										}}
									/>
								</Icon>
								<Typography
									variant="h4"
									className={styles.Home_TableTitle_Title_Text}
								>
									Contacts List
								</Typography>
							</div>
						</div>
						<div>
							<Toolbar
								disableGutters="true"
								variant="dense"
								className={styles.Home_TableToolbar}
							>
								<TextField
									onChange={handleSearch}
									InputProps={{
										startAdornment: (
											<InputAdornment position="start">
												<Search />
											</InputAdornment>
										),
									}}
									size="small"
									className={styles.Home_TableToolbar_Searchbar}
									placeholder="Enter search term"
								/>
								<Button
									variant="contained"
									className={styles.Home_TableToolbar_Addbutton}
									startIcon={<Add />}
									onClick={() => {
										setSelectedContact(null);
										setOpenPopup(true);
									}}
								>
									Add Contact
								</Button>
							</Toolbar>
						</div>
						<div className={styles.Home_TableTitle_TableContainer}>
							<TblContainer>
								<TblHead />
								{isLoading && (
									<TableRow>
										<TableCell className={styles.Home_TableTitle_TableCell}>
											<PulseLoader />
										</TableCell>
										<TableCell className={styles.Home_TableTitle_TableCell}>
											<PulseLoader />
										</TableCell>
										<TableCell className={styles.Home_TableTitle_TableCell}>
											<PulseLoader />
										</TableCell>
										<TableCell className={styles.Home_TableTitle_TableCell}>
											<PulseLoader />
										</TableCell>
										<TableCell className={styles.Home_TableTitle_TableCell}>
											<PulseLoader />
										</TableCell>
										<TableCell className={styles.Home_TableTitle_TableCell}>
											<PulseLoader />
										</TableCell>
										<TableCell className={styles.Home_TableTitle_TableCell}>
											<PulseLoader />
										</TableCell>
									</TableRow>
								)}
								{!isLoading && (
									<TableBody>
										{contactsAfterPagingAndSorting().map((item) => (
											<TableRow key={item.id}>
												<TableCell className={styles.Home_TableTitle_TableCell}>
													{item.emailAddress}
												</TableCell>
												<TableCell className={styles.Home_TableTitle_TableCell}>
													{item.firstName}
												</TableCell>
												<TableCell className={styles.Home_TableTitle_TableCell}>
													{item.lastName}
												</TableCell>
												<TableCell className={styles.Home_TableTitle_TableCell}>
													{item.contactNumber}
												</TableCell>
												<TableCell className={styles.Home_TableTitle_TableCell}>
													{item.billingAddress}
												</TableCell>
												<TableCell className={styles.Home_TableTitle_TableCell}>
													{item.deliveryAddress}
												</TableCell>
												<TableCell className={styles.Home_TableTitle_TableCell}>
													<div style={{ display: "flex", gap: "1vw" }}>
														<Button
															color="primary"
															variant="contained"
															onClick={() => {
																setSelectedContact(item);
																setOpenPopup(true);
															}}
														>
															<EditOutlined />
														</Button>
														<Button
															color="secondary"
															variant="contained"
															onClick={() => {
																setSelectedContact(item);
																setOpenPopupDelete(true);
															}}
														>
															<CloseOutlined />
														</Button>
													</div>
												</TableCell>
											</TableRow>
										))}
									</TableBody>
								)}
							</TblContainer>
						</div>
						<TblPagination />
					</Paper>
					<Popup
						title={selectedContact ? "Edit Contact" : "Add Contact"}
						openPopup={openPopup}
						setOpenPopup={setOpenPopup}
					>
						<ContactForm
							setOpenPopup={setOpenPopup}
							contactForEdit={selectedContact}
							setContactsList={setContactsList}
						/>
					</Popup>
					<Popup
						title="Delete Contact"
						openPopup={openPopupDelete}
						setOpenPopup={setOpenPopupDelete}
					>
						<ConfirmDialog
							setOpenPopupDelete={setOpenPopupDelete}
							setContactsList={setContactsList}
							contactForDelete={selectedContact}
						/>
					</Popup>
				</>
			</div>
		</>
	);
};

export default Home;
