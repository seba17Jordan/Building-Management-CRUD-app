export interface Invitation {
    id?: string;
    email?: string;
    name?: string;

    expirationDate?: Date;
    role: number;
    state?: number;
  }
