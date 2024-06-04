import { Owner } from "./owner.model";

export interface Apartment {
    Id?: string;
    Floor: number;
    Number: number;
    Owner: Owner;
    Rooms: number;
    Bathrooms: number;
    HasTerrace: boolean;
  }