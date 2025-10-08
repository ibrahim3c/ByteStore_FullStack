export interface Category {
  id: number;
  name: string;
  description?: string;
  parentCategoryId?: number;
  parentCategory?: Category; // optional reference
  subCategories?: Category[]; // nested subcategories
}
