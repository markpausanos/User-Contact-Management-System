import { useState, useEffect } from "react";

import { ContactsService } from "../services";

const useContacts = () => {
	const [isLoading, setIsLoading] = useState(true);
	const [contacts, setContacts] = useState([]);

	useEffect(() => {
		const getContacts = async () => {
			const { data: getContactsResponse } = await ContactsService.list();

			if (getContactsResponse) {
				setContacts(getContactsResponse);
			}

			setIsLoading(false);
		};

		getContacts();
	}, []);

	return { isLoading, contacts };
};

export default useContacts;
