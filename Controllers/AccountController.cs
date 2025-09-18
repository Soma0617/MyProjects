using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalExamProject.Models;

namespace FinalExamProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly GoShopContext _context;

        public AccountController(GoShopContext context)
        {
            _context = context;
        }

        // GET: Account
        public async Task<IActionResult> Index([FromQuery] AccountQueryVm query)
        {
            var q = _context.Account
                .Include(a => a.MemberInfo)     // ← 改這裡
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.Q))
            {
                var key = query.Q.Trim();
                q = q.Where(a =>
                    a.AccountID.Contains(key) ||
                    (a.MemberInfo != null && a.MemberInfo.Email.Contains(key))  // ← 改這裡
                );
            }

            q = query.Sort switch
            {
                "account_desc" => q.OrderByDescending(a => a.AccountID),
                _ => q.OrderBy(a => a.AccountID)
            };

            var total = await q.CountAsync();
            var pageSize = Math.Clamp(query.PageSize, 5, 50);
            var page = Math.Max(1, query.Page);
            var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            ViewBag.Total = total;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.Sort = query.Sort;
            ViewBag.Query = query;

            return View(items);
        }

        // GET: Account/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var account = await _context.Account
                .Include(a => a.MemberInfo)   // ← 改這裡
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.AccountID == id);

            if (account == null) return NotFound();

            return View(account);
        }

        // GET: Account/Create
        public IActionResult Create()
        {
            ViewBag.Members = _context.Member.AsNoTracking().ToList();
            return View(new AccountCreateVm());
        }

        // POST: Account/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccountCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Members = _context.Member.AsNoTracking().ToList();
                return View(vm);
            }

            var entity = new Account
            {
                AccountID = vm.AccountID.Trim(),
                PasswordHash = vm.PasswordHash,
                MemberID = vm.MemberID
            };

            _context.Account.Add(entity);
            await _context.SaveChangesAsync();
            TempData["ok"] = "帳號建立成功";
            return RedirectToAction(nameof(Index));
        }

        // GET: Account/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var account = await _context.Account.FindAsync(id);
            if (account == null) return NotFound();

            var vm = new AccountEditVm
            {
                AccountID = account.AccountID,
                PasswordHash = account.PasswordHash,
                MemberID = account.MemberID
            };

            ViewBag.Members = _context.Member.AsNoTracking().ToList();
            return View(vm);
        }

        // POST: Account/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, AccountEditVm vm)
        {
            if (id != vm.AccountID) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewBag.Members = _context.Member.AsNoTracking().ToList();
                return View(vm);
            }

            var account = await _context.Account.FirstOrDefaultAsync(a => a.AccountID == id);
            if (account == null) return NotFound();

            account.PasswordHash = vm.PasswordHash;
            account.MemberID = vm.MemberID;

            try
            {
                await _context.SaveChangesAsync();
                TempData["ok"] = "帳號已更新";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Account.Any(e => e.AccountID == id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Account/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var account = await _context.Account
                .Include(a => a.MemberInfo)   // ← 改這裡
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.AccountID == id);

            if (account == null) return NotFound();

            return View(account);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var account = await _context.Account.FindAsync(id);
            if (account != null)
            {
                _context.Account.Remove(account);
                await _context.SaveChangesAsync();
                TempData["ok"] = "帳號已刪除";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
