using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Domain;
using DataAccess;

public class PreventDoubleVoteAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var user = context.HttpContext.User;
        if (!user.Identity.IsAuthenticated)
        {
            context.Result = new RedirectToActionResult("Login", "Account", null);
            return;
        }

        if (context.ActionArguments.TryGetValue("id", out var pollIdObj) && pollIdObj is int pollId)
        {
            var repo = context.HttpContext.RequestServices.GetService(typeof(IPollRepository)) as IPollRepository;
            var poll = repo?.GetPolls().FirstOrDefault(p => p.Id == pollId);
            var userId = user.Identity?.Name;

            if (poll != null && poll.VoterIds.Contains(userId))
            {
                context.Result = new RedirectToActionResult("AlreadyVoted", "Poll", null);
            }
        }
    }
}
