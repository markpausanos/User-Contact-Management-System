import { useContext } from "react";
import PropTypes from "prop-types";
import { Outlet, Navigate } from "react-router-dom";
import { UserContext } from "../contexts";

const NoAuthRoute = ({ children }) => {
	const { user } = useContext(UserContext);

	// The page will be changed to home
	if (user) {
		return <Navigate to="/home" replace />;
	}

	return children ? children : <Outlet />;
};

export default NoAuthRoute;

NoAuthRoute.propTypes = {
	children: PropTypes.node,
};
