using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace DataAccess
{
    public class SessionRepository : ISessionRepository
    {
        private readonly DbContext _context;
        public SessionRepository (DbContext context)
        {
            _context = context;
        }

        public Session GetSessionByToken(Guid token)
        {
            return _context.Set<Session>().FirstOrDefault(s => s.Token == token);
        }

        public void Insert(Session actualSession)
        {
            _context.Entry(actualSession.User).State = EntityState.Unchanged;
            _context.Set<Session>().Add(actualSession);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
