import { createContext } from "react";

const UserContext = createContext({
	user: null,
	loginUpdate: () => {},
	loginRestart: () => {},
});

export default UserContext;
