import axios from "axios";
import config from "./config";

const BASE_URL = `${config.API_URL}/Contacts`;

const ContactsService = {
	create: (contact) => axios.post(BASE_URL, contact, { withCredentials: true }),
	list: () => axios.get(BASE_URL),
	retrieveById: (id) => axios.get(`${BASE_URL}/${id}`),
	update: (id, contactUpdate) => axios.put(`${BASE_URL}/${id}`, contactUpdate),
	delete: (id) => axios.delete(`${BASE_URL}/${id}`),
};

export default ContactsService;
