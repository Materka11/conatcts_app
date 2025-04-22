export const API_URL =
  import.meta.env.VITE_API_URL || "http://localhost:5025/api";

export const AUTH_URL = `${API_URL}/Auth`;
export const CONTACTS_URL = `${API_URL}/Contacts`;
export const CATEGORIES_URL = `${API_URL}/Categories`;
export const SUBCATEGORIES_URL = `${API_URL}/Subcategories`;
