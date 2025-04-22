import { client } from "./middleware";
import { CONTACTS_URL } from "./endpoints";

export const getContacts = async () => {
  try {
    const response = await client.get<IContact[]>(`${CONTACTS_URL}`);
    return response.data;
  } catch (err) {
    console.error(err);
  }
};

export const getContact = async (contactId: string) => {
  try {
    const response = await client.get<IContact>(`${CONTACTS_URL}/${contactId}`);
    return response.data;
  } catch (err) {
    console.error(err);
  }
};

export const createContact = async (contact: IContactBody) => {
  try {
    const response = await client.post<IContact>(`${CONTACTS_URL}`, contact);
    return response.data;
  } catch (err) {
    console.error(err);
  }
};

export const updateContact = async (
  contactId: string,
  contact: IContactBody
) => {
  try {
    const response = await client.put<IContact>(
      `${CONTACTS_URL}/${contactId}`,
      contact
    );
    return response.data;
  } catch (err) {
    console.error(err);
  }
};

export const deleteContact = async (contactId: string) => {
  try {
    const response = await client.delete(`${CONTACTS_URL}/${contactId}`);
    return response.data;
  } catch (err) {
    console.error(err);
  }
};
