import { Owner } from "./owner.model";

export interface Apartment {
    id?: string;
    floor: number;
    number: number;
    owner: Owner;
    rooms: number;
    bathrooms: number;
    hasTerrace: boolean;
  }