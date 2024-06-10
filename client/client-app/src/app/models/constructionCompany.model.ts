import { User } from './user.model';

export interface ConstructionCompany {
    id?: string;
    name: string;
    constructionCompanyAdmin?: User;
}
