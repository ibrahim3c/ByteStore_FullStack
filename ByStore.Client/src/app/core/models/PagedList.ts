import { MetaData } from "./MetaData";

export interface PagedList<T> {
  metaData: MetaData;
  items: T[];
}
