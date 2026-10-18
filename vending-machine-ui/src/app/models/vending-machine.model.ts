export enum ProductType {
  Cola = 1,
  Water = 2,
  Snack = 3
}

export interface Shelf {
  id: string;
  productType: ProductType;
  maxCapacity: number;
  currentQuantity: number;
}

export interface VendingMachine {
  id: string;
  name: string;
  location?: string;
  maxShelves: number;
  shelves: Shelf[];
}