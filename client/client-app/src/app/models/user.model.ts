export interface User {
    id?: string;
    email: string;
    password: string;
    name: string;
    lastName?: string;
    role: Roles;
  }
  
  export enum Roles {
    Administrator = 'Administrator',
    User = 'User'
  }
  