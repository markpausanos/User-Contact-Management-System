import { useState, useContext } from "react";
import { Link } from "react-router-dom";
import { UserContext } from "../../contexts";
import styles from "./styles.module.scss";
import { IconButton } from "@mui/material";
import { Menu, MenuOpen } from "@mui/icons-material";
import { NavbarData } from "./navbarData";

const NavHeader = () => {
	const userContext = useContext(UserContext);
	const user = userContext.user.user;

	const [sidebar, setSidebar] = useState(false);
	const showSidebar = () => setSidebar(!sidebar);

	console.log(sidebar);

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
					{NavbarData.map((item, index) => {
						return (
							<li key={index} className={styles.NavMenu_Text}>
								<Link to={item.path}>{item.title}</Link>
							</li>
						);
					})}
				</ul>
			</nav>
		</>
	);
};

export default NavHeader;
