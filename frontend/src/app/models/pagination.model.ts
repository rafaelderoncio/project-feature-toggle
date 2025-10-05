import { Feature } from "./feature.model";

export interface Pagination {
  items: Feature[];
  totalRecords: number;
  totalPages: number;
  page: number;
  quantity: number;
  previousPage: any;
  nextPage: any;
}