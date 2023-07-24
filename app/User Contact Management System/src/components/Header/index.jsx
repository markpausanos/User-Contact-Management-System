import phone from "../../statics/images/phone.png";
import styles from "./styles.module.scss";
import { Avatar } from "@mui/material";

const Header = () => {
	return (
		<>
			<div className={styles.Header}>
				<div>
					<Avatar
						className={styles.Header_Logo}
						alt="LinkUp"
						sx={{ height: "5.5vh", width: "5.5vh" }}
						src={phone}
						variant="circular"
					/>
				</div>
				<div>
					<h2>LinkUp</h2>
				</div>
			</div>
		</>
	);
};

export default Header;
