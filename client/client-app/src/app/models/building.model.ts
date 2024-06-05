import { Apartment } from "./apartment.model";

export interface Building {
    id: number;
    name: string;
    address: string;
    constructionCompany: string;
    commonExpenses: number;
    hasManager: boolean;
    managerName: string;
    apartements: Apartment[];
  }