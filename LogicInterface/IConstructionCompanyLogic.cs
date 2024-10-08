﻿using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicInterface
{
    public interface IConstructionCompanyLogic
    {
        ConstructionCompany CreateConstructionCompany(ConstructionCompany constructionCompany);
        ConstructionCompany GetConstructionCompany(User constructionCompanyAdmin);
        ConstructionCompany UpdateConstructionCompanyName(string newName, User constructionCompanyAdmin);
    }
}
