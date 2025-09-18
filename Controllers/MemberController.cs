using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalExamProject.Models;

namespace FinalExamProject.Controllers
{
    public class MemberController : Controller
    {
        private readonly GoShopContext _context;

        public MemberController(GoShopContext context)
        {
            _context = context;
        }

        // GET: Member
        public async Task<IActionResult> Index([FromQuery] MemberQueryVm query)
        {
            var q = _context.Member.AsNoTracking();

            // 關鍵字搜尋（姓名 / Email / 電話）
            if (!string.IsNullOrWhiteSpace(query.Q))
            {
                var key = query.Q.Trim();
                q = q.Where(m =>
                    m.Name.Contains(key) ||
                    m.Email.Contains(key) ||
                    (m.Phone != null && m.Phone.Contains(key)));
            }

            // Email 驗證狀態
            if (query.IsEmailConfirmed.HasValue)
            {
                q = q.Where(m => m.IsEmailConfirmed == query.IsEmailConfirmed.Value);
            }

            // 排序
            q = query.Sort switch
            {
                "name" => q.OrderBy(m => m.Name),
                "name_desc" => q.OrderByDescending(m => m.Name),
                "email" => q.OrderBy(m => m.Email),
                "email_desc" => q.OrderByDescending(m => m.Email),
                "created" => q.OrderBy(m => m.CreatedDate),
                _ => q.OrderByDescending(m => m.CreatedDate) // created_desc(預設)
            };

            // 分頁
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

        // GET: Member/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var member = await _context.Member
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.MemberID == id);

            if (member == null) return NotFound();

            return View(member);
        }

        // GET: Member/Create
        public IActionResult Create() => View(new MemberCreateVm());

        // POST: Member/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MemberCreateVm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var entity = new Member
            {
                MemberID = Guid.NewGuid(),
                Name = vm.Name.Trim(),
                Birthday = vm.Birthday,
                Sex = string.IsNullOrWhiteSpace(vm.Sex) ? null : vm.Sex.Trim(),
                Phone = string.IsNullOrWhiteSpace(vm.Phone) ? null : vm.Phone.Trim(),
                Address = string.IsNullOrWhiteSpace(vm.Address) ? null : vm.Address.Trim(),
                Email = vm.Email.Trim(),
                IsEmailConfirmed = vm.IsEmailConfirmed,
                CreatedDate = DateTime.Now,
                LastLoginDate = null
            };

            _context.Member.Add(entity);
            await _context.SaveChangesAsync();
            TempData["ok"] = "會員建立成功";
            return RedirectToAction(nameof(Index));
        }

        // GET: Member/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var member = await _context.Member.FindAsync(id);
            if (member == null) return NotFound();

            var vm = new MemberEditVm
            {
                MemberID = member.MemberID,
                Name = member.Name,
                Birthday = member.Birthday,
                Sex = member.Sex,
                Phone = member.Phone,
                Address = member.Address,
                Email = member.Email,
                IsEmailConfirmed = member.IsEmailConfirmed
            };

            return View(vm);
        }

        // POST: Member/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, MemberEditVm vm)
        {
            if (id != vm.MemberID) return NotFound();
            if (!ModelState.IsValid) return View(vm);

            var member = await _context.Member.FirstOrDefaultAsync(m => m.MemberID == id);
            if (member == null) return NotFound();

            member.Name = vm.Name.Trim();
            member.Birthday = vm.Birthday;
            member.Sex = string.IsNullOrWhiteSpace(vm.Sex) ? null : vm.Sex.Trim();
            member.Phone = string.IsNullOrWhiteSpace(vm.Phone) ? null : vm.Phone.Trim();
            member.Address = string.IsNullOrWhiteSpace(vm.Address) ? null : vm.Address.Trim();
            member.Email = vm.Email.Trim();
            member.IsEmailConfirmed = vm.IsEmailConfirmed;

            try
            {
                await _context.SaveChangesAsync();
                TempData["ok"] = "會員資料已更新";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Member.AnyAsync(e => e.MemberID == vm.MemberID))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Member/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var member = await _context.Member
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.MemberID == id);

            if (member == null) return NotFound();

            return View(member);
        }

        // POST: Member/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var member = await _context.Member.FindAsync(id);
            if (member != null)
            {
                _context.Member.Remove(member);
                await _context.SaveChangesAsync();
                TempData["ok"] = "會員已刪除";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
