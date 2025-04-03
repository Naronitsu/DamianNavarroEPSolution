using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain;

namespace DataAccess
{
    public class PollRepository
    {
        private readonly PollDbContext _context;

        public PollRepository(PollDbContext context)
        {
            _context = context;
        }

        public void CreatePoll(Poll poll)
        {
            poll.DateCreated = DateTime.Now;
            _context.Polls.Add(poll);
            _context.SaveChanges();
        }

        public IEnumerable<Poll> GetPolls()
        {
            return _context.Polls.OrderByDescending(p => p.DateCreated).ToList();
        }
    }
}