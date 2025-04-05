namespace Presentation.Models
{
    public class PollResultsViewModel
    {
        public string Title { get; set; }
        public List<string> Labels { get; set; }
        public List<int> VoteCounts { get; set; }
    }
}
