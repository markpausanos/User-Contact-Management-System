import { useState, useEffect, useContext } from "react";
import { UserContext } from "../../contexts";
import styles from "./styles.module.scss";
import { Button, IconButton, Typography } from "@mui/material";
import { Menu, MenuOpen } from "@mui/icons-material";
import UserForm from "./UserForm";
import Popup from "../Popup";
import PasswordForm from "./PasswordForm";

const NavHeader = () => {
	const userContext = useContext(UserContext);
	const [user, setUser] = useState(userContext.user);

	const [sidebar, setSidebar] = useState(false);
	const [openAccountPopup, setOpenAccountPopup] = useState(false);
	const [openPasswordPopup, setOpenPasswordPopup] = useState(false);
	const showSidebar = () => setSidebar(!sidebar);

	useEffect(() => {
		userContext.loginUpdate(user);
	}, [user, userContext]);
	return (
		<>
			<div>
				<div className={styles.NavHeader}>
					<div>
						<IconButton
							color="primary"
							alt="LinkUp"
							size="large"
							onClick={showSidebar}
						>
							{!sidebar && <Menu />}
							{sidebar && <MenuOpen />}
						</IconButton>
					</div>

					<div>
						<div>
							<h2>
								Hello, {user.firstName} {user.lastName}
							</h2>
						</div>
					</div>
				</div>
			</div>

			<nav className={!sidebar ? styles.NavMenu : styles.NavMenu_Active}>
				<ul className={styles.NavMenu_Item} onClick={showSidebar}>
					<li className={styles.NavMenu_Text}>
						<Button
							href="#"
							variant="text"
							size="small"
							sx={{ padding: 0 }}
							onClick={() => setOpenAccountPopup(true)}
						>
							<Typography margin="dense" variant="h6">
								Account
							</Typography>
						</Button>
					</li>
					<li className={styles.NavMenu_Text}>
						<Button
							href="#"
							variant="text"
							size="small"
							sx={{ padding: 0 }}
							onClick={() => setOpenPasswordPopup(true)}
						>
							<Typography margin="dense" variant="h6">
								Password
							</Typography>
						</Button>
					</li>
					<li className={styles.NavMenu_Text}>
						<Button
							href="#"
							variant="text"
							size="small"
							sx={{ padding: 0 }}
							onClick={() => userContext.loginRestart()}
						>
							<Typography margin="dense" variant="h6">
								Logout
							</Typography>
						</Button>
					</li>
				</ul>
			</nav>

			<Popup
				title="Update User Details"
				openPopup={openAccountPopup}
				setOpenPopup={setOpenAccountPopup}
			>
				<UserForm
					setOpenPopupUser={setOpenAccountPopup}
					setUser={setUser}
					userForEdit={user}
				/>
			</Popup>
			<Popup
				title="Update Password"
				openPopup={openPasswordPopup}
				setOpenPopup={setOpenPasswordPopup}
			>
				<PasswordForm setOpenPopupPassword={setOpenPasswordPopup} />
			</Popup>
		</>
	);
};

export default NavHeader;
