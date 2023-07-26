import { useState, useEffect } from "react";
import { ContactsService } from "../services";

const useContacts = () => {
	const [isLoading, setIsLoading] = useState(true);
	const [contacts, setContacts] = useState([]);

	useEffect(() => {
		const getContacts = async () => {
			try {
				const { data: getContactsResponse } = await ContactsService.list();

				if (getContactsResponse) {
					setContacts(getContactsResponse);
				}
			} catch (error) {
				// Handle any errors that occur during the API call
				console.error("Error fetching contacts:", error);
			} finally {
				setIsLoading(false);
			}
		};

		getContacts();
	}, []);

	const refreshContacts = async () => {
		setIsLoading(true);
		try {
			const { data: getContactsResponse } = await ContactsService.list();

			if (getContactsResponse) {
				setContacts(getContactsResponse);
			}
		} catch (error) {
			console.error("Error fetching contacts:", error);
		} finally {
			setIsLoading(false);
		}
	};

	useEffect(() => {
		refreshContacts();
	}, []);

	return { contacts, isLoading, refreshContacts };
};

export default useContacts;
