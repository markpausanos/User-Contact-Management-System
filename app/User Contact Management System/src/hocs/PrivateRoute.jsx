import { useContext } from "react";
import { Route, Navigate } from "react-router-dom";
import { UserContext } from "../contexts";

const PrivateRoute = ({ ...rest }) => {
	const { user } = useContext(UserContext);

	// The page can be accessed by the user
	if (user) {
		return <Route {...rest} />;
	}

	return <Route name="Login" render={() => <Navigate to="/login" />} />;
};

export default PrivateRoute;
