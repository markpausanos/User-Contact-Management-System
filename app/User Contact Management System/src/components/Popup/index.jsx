import {
	Dialog,
	DialogTitle,
	DialogContent,
	Typography,
	Button,
} from "@mui/material";
import { Close } from "@mui/icons-material";

const Popup = ({ title, children, openPopup, setOpenPopup }) => {
	return (
		<Dialog open={openPopup} maxWidth="md">
			<DialogTitle>
				<div style={{ display: "flex" }}>
					<Typography variant="h6" component="div" style={{ flexGrow: 1 }}>
						{title}
					</Typography>
					<Button
						color="secondary"
						onClick={() => {
							setOpenPopup(false);
						}}
					>
						<Close />
					</Button>
				</div>
			</DialogTitle>
			<DialogContent dividers>{children}</DialogContent>
		</Dialog>
	);
};

export default Popup;
