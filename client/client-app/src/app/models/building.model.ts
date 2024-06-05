import { Apartment } from "./apartment.model";

export interface Building {
    Id: number;
    name: string;
    address: string;
    hasManager: boolean;
    managerName: string;
  }