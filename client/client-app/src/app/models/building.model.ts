import { Apartment } from "./apartment.model";

export interface Building {
    id: number;
    name: string;
    address: string;
    hasManager: boolean;
    managerName: string;
  }