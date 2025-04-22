import { CATEGORIES_URL } from "./endpoints";
import { client } from "./middleware";

export const getCategories = async () => {
  try {
    const response = await client.get<ICategory[]>(`${CATEGORIES_URL}`);
    return response.data;
  } catch (err) {
    console.error(err);
  }
};
