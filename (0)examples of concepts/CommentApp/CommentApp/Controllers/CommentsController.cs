using CommentApp.Data;
using CommentApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommentApp.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Comments  (or /Comments/Index?thread=xyz)
        public async Task<IActionResult> Index(string thread = "default")
        {
            var comments = await _context.Comments
                .Include(c => c.User)
                .Where(c => c.ThreadKey == thread)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            ViewBag.ThreadKey = thread;
            return View(comments);
        }

        // POST: /Comments/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string content, string thread = "default")
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                TempData["Error"] = "Comment cannot be empty.";
                return RedirectToAction(nameof(Index), new { thread });
            }

            var userId = _userManager.GetUserId(User);
            if (userId == null) return Challenge();

            var comment = new Comment
            {
                Content = content.Trim(),
                UserId = userId,
                ThreadKey = thread,
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { thread });
        }

        // POST: /Comments/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string content)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (comment.UserId != userId && !User.IsInRole("Admin"))
                return Forbid();

            if (string.IsNullOrWhiteSpace(content))
            {
                TempData["Error"] = "Comment cannot be empty.";
                return RedirectToAction(nameof(Index), new { thread = comment.ThreadKey });
            }

            comment.Content = content.Trim();
            comment.EditedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { thread = comment.ThreadKey });
        }

        // POST: /Comments/Delete/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (comment.UserId != userId && !User.IsInRole("Admin"))
                return Forbid();

            var thread = comment.ThreadKey;
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { thread });
        }
    }
}