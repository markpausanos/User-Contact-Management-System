import { useContext } from "react";
import { Route, Redirect } from "react-router-dom";
import { UserContext } from "../contexts";

const NoAuthRoute = ({ ...rest }) => {
	const { user } = useContext(UserContext);

	if (user) {
		return <Route name="Student" render={() => <Redirect to="/student" />} />;
	}

	// the page can be accessed by the user
	return <Route {...rest} />;
};

export default NoAuthRoute;
