using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain;

namespace DataAccess
{
    public class PollRepository : IPollRepository
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

        public void Vote(int pollId, int optionSelected)
        {
            var poll = _context.Polls.Find(pollId);
            if (poll == null) return;

            switch (optionSelected)
            {
                case 1: poll.Option1VotesCount++; break;
                case 2: poll.Option2VotesCount++; break;
                case 3: poll.Option3VotesCount++; break;
            }

            _context.SaveChanges();
        }

        public void UpdatePoll(Poll poll)
        {
            _context.Polls.Update(poll);
            _context.SaveChanges();
        }


    }
}