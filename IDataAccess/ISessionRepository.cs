﻿using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDataAccess
{
    public interface ISessionRepository
    {
        void Delete(Session currentSession);
        Session GetSessionByToken(Guid token);
        void Insert(Session actualSession);
        void Save();
    }
}
