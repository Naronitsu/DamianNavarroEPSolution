using Domain;

namespace Domain
{
    public interface IPollRepository
    {
        void CreatePoll(Poll poll);
        IEnumerable<Poll> GetPolls();
        void Vote(int pollId, int optionSelected);
    }
}
