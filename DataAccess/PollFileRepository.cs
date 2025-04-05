using Domain;
using System.Text.Json;

namespace DataAccess
{
    public class PollFileRepository : IPollRepository
    {
        private readonly string _filePath = "polls.json";

        public void CreatePoll(Poll poll)
        {
            List<Poll> polls = GetPolls().ToList();
            poll.Id = polls.Any() ? polls.Max(p => p.Id) + 1 : 1;
            poll.DateCreated = DateTime.Now;

            polls.Add(poll);
            File.WriteAllText(_filePath, JsonSerializer.Serialize(polls));
        }

        public IEnumerable<Poll> GetPolls()
        {
            if (!File.Exists(_filePath))
                return new List<Poll>();

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Poll>>(json) ?? new List<Poll>();
        }

        public void Vote(int pollId, int optionSelected)
        {
            var polls = GetPolls().ToList();
            var poll = polls.FirstOrDefault(p => p.Id == pollId);
            if (poll == null) return;

            switch (optionSelected)
            {
                case 1: poll.Option1VotesCount++; break;
                case 2: poll.Option2VotesCount++; break;
                case 3: poll.Option3VotesCount++; break;
            }

            File.WriteAllText(_filePath, JsonSerializer.Serialize(polls));
        }

        public void UpdatePoll(Poll updatedPoll)
        {
            var polls = GetPolls().ToList();
            var index = polls.FindIndex(p => p.Id == updatedPoll.Id);

            if (index != -1)
            {
                polls[index] = updatedPoll;
                File.WriteAllText(_filePath, JsonSerializer.Serialize(polls));
            }
        }


    }
}
