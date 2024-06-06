import { Apartment } from "./apartment.model";

export interface Building {
    id?: string;
    name: string;
    address: string;
    constructionCompany: string;
    commonExpenses: number;
    hasManager: boolean;
    managerName?: string;
    apartments: Apartment[];
  }