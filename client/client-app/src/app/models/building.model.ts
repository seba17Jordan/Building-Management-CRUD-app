import { Apartment } from "./apartment.model";

export interface Building {
    Id: number;
    Name: string;
    Address: string;
    ConstructionCompany: string;
    CommonExpenses: number;
    Apartments: Apartment[];
  }