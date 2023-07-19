import { createContext } from "react";

const UserContext = createContext({
	user: {},
	loginUpdate: () => {},
	loginRestart: () => {},
});

export default UserContext;
