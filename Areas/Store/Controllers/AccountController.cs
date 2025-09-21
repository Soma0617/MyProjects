using FinalExamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExamProject.Areas.Store.Controllers
{
    [Area("Store")]
    public class AccountController : Controller
    {
        private readonly GoShopContext _context;

        public AccountController(GoShopContext context)
        {
            _context = context;
        }

        // GET: /Store/Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            return View(new LoginVm { ReturnUrl = returnUrl });
        }

        // POST: /Store/Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var account = await _context.Account
                .Include(a => a.MemberInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.AccountID == vm.Email);

            // 這裡為了簡化示範，直接比對「純文字密碼」
            if (account == null || account.PasswordHash != vm.Password)
            {
                ModelState.AddModelError("", "帳號或密碼錯誤");
                return View(vm);
            }

            // 設定 Session（登入成功）
            HttpContext.Session.SetString("LOGIN_MEMBER_ID", account.MemberID.ToString());
            HttpContext.Session.SetString("LOGIN_MEMBER_NAME", account.MemberInfo.Name);

            if (!string.IsNullOrWhiteSpace(vm.ReturnUrl))
                return LocalRedirect(vm.ReturnUrl);

            TempData["ok"] = "登入成功";
            return RedirectToAction("Index", "Home", new { area = "Store" });
        }

        // GET: /Store/Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterVm());
        }

        // POST: /Store/Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            // Email 不可重複
            var exists = await _context.Member.AnyAsync(m => m.Email == vm.Email);
            if (exists)
            {
                ModelState.AddModelError(nameof(vm.Email), "此 Email 已被註冊");
                return View(vm);
            }

            // 建立 Member
            var member = new Member
            {
                MemberID = Guid.NewGuid(),
                Name = vm.Name.Trim(),
                Email = vm.Email.Trim(),
                Phone = vm.Phone ?? "",
                Address = vm.Address ?? "",
                CreatedDate = DateTime.Now,
                IsEmailConfirmed = false
            };

            // 建立 Account（將 Email 當作帳號）
            var account = new Account
            {
                AccountID = vm.Email.Trim(),
                MemberID = member.MemberID,
                PasswordHash = vm.Password,        // 簡化：直接存明碼
                CreatedDate = DateTime.Now,
                LastLoginDate = DateTime.Now
            };

            _context.Member.Add(member);
            _context.Account.Add(account);
            await _context.SaveChangesAsync();

            // 設定 Session（自動登入）
            HttpContext.Session.SetString("LOGIN_MEMBER_ID", member.MemberID.ToString());
            HttpContext.Session.SetString("LOGIN_MEMBER_NAME", member.Name);

            TempData["ok"] = "註冊成功，已自動登入";
            return RedirectToAction("Index", "Home", new { area = "Store" });
        }

        // POST: /Store/Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("LOGIN_MEMBER_ID");
            HttpContext.Session.Remove("LOGIN_MEMBER_NAME");
            TempData["ok"] = "您已登出";
            return RedirectToAction("Index", "Home", new { area = "Store" });
        }

        // GET: /Store/Account/Profile（示範用）
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var sid = HttpContext.Session.GetString("LOGIN_MEMBER_ID");
            if (!Guid.TryParse(sid, out var memberId))
                return RedirectToAction("Login", new { returnUrl = Url.Action("Profile", "Account", new { area = "Store" }) });

            var member = await _context.Member.AsNoTracking().FirstOrDefaultAsync(m => m.MemberID == memberId);
            if (member == null) return RedirectToAction("Login");

            return View(member);
        }
    }
}
