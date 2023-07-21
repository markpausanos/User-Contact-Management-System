import { useContext } from "react";
import PropTypes from "prop-types";
import { Navigate, Outlet } from "react-router-dom";
import { UserContext } from "../contexts";

const PrivateRoute = ({ children }) => {
	const { user } = useContext(UserContext);

	// The page can be accessed by the user
	if (user) {
		return children ? children : <Outlet />;
	}

	return <Navigate to="/login" replace />;
};

export default PrivateRoute;

PrivateRoute.propTypes = {
	children: PropTypes.node,
};
