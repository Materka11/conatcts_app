import { SUBCATEGORIES_URL } from "./endpoints";
import { client } from "./middleware";

export const getSubcategories = async () => {
  try {
    const response = await client.get<ISubcategory[]>(`${SUBCATEGORIES_URL}`);
    return response.data;
  } catch (err) {
    console.error(err);
  }
};
