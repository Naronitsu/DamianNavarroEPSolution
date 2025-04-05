using DataAccess;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class PollController : Controller
    {
        private readonly IPollRepository _pollRepository;

        public PollController(IPollRepository pollRepository)
        {
            _pollRepository = pollRepository;
        }

        public IActionResult Index()
        {
            var polls = _pollRepository.GetPolls();
            return View(polls);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AlreadyVoted([FromServices] IPollRepository injectedRepo)
        {
            var allPolls = injectedRepo.GetPolls();
            return View();
        }


        [HttpPost]
        public IActionResult Create(Poll poll)
        {
            if (ModelState.IsValid)
            {
                _pollRepository.CreatePoll(poll);
                return RedirectToAction("Index");
            }
            return View(poll);
        }

        [Authorize]
        [HttpGet]
        [PreventDoubleVote]
        public IActionResult Vote(int id)
        {
            var poll = _pollRepository.GetPolls().FirstOrDefault(p => p.Id == id);
            return View(poll);
        }

        [HttpPost]
        [Authorize]
        [PreventDoubleVote]
        public IActionResult Vote(int id, int optionSelected)
        {
            var userId = User.Identity?.Name;
            var poll = _pollRepository.GetPolls().FirstOrDefault(p => p.Id == id);

            if (poll == null || poll.VoterIds.Contains(userId))
                return RedirectToAction("AlreadyVoted");

            _pollRepository.Vote(id, optionSelected);
            poll.VoterIds.Add(userId);
            _pollRepository.UpdatePoll(poll);

            ViewBag.Labels = new[] { poll.Option1Text, poll.Option2Text, poll.Option3Text };
            ViewBag.Votes = new[] { poll.Option1VotesCount, poll.Option2VotesCount, poll.Option3VotesCount };

            return View("Results", poll);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Results(int id)
        {
            var poll = _pollRepository.GetPolls().FirstOrDefault(p => p.Id == id);
            if (poll == null)
            {
                return NotFound();
            }
            return View(poll);
        }






    }
}
