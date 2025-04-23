interface IContact {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  categoryId: string;
  category: ICategory;
  subcategoryId: string;
  subcategory: ISubcategory;
  phone: string;
  dateOfBirth: string;
}

interface IContactBody {
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  dateOfBirth: string;
  categoryId: string;
  subcategoryId?: string;
  customSubcategory?: string;
  category?: ICategory;
  subcategory?: ISubcategory;
}
