export interface ProductReview {
  comment: string;
  rating: number;
  createdOn: string; // API sends ISO string (e.g. "2025-10-16T10:00:00Z")
  customerName: string;
}
