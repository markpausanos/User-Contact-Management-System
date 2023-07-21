import styles from "./styles.module.scss";
import { Button } from "@mui/material";
import { useNavigate } from "react-router-dom";

const ScreenNoExist = () => {
	const navigate = useNavigate();
	return (
		<>
			<div className={styles.ScreenNoExists}>
				<p>Page does not exist!</p>
				<br />
				<Button
					variant="contained"
					type="submit"
					color="primary"
					onClick={() => navigate("/home")}
				>
					Back to Home?
				</Button>
			</div>
		</>
	);
};

export default ScreenNoExist;
