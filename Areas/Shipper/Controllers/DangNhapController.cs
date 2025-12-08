using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebDatMonAn.Models.ViewModel;
using WebDatMonAn.Repository.Extension;
using WebDatMonAn.Repository;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.EntityFrameworkCore;
using WebDatMonAn.Repository.Services;

namespace WebDatMonAn.Areas.Shipper.Controllers
{
    [Area("Shipper")]
    public class DangNhapController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly INotyfService _notyfService;
        private readonly EmailServices _emailService;
        public DangNhapController(DataContext dataContext, INotyfService notyfService, EmailServices emailServices)

        {
            _notyfService = notyfService;
            _dataContext = dataContext;
            _emailService = emailServices;
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login, string returnUrl = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isEmail = Helper.IsValidEmail(login.Email);
                    if (!isEmail)
                    {
                        _notyfService.Error("Email không hợp lệ!");
                        return PartialView(login);
                    }

                    var shipper = _dataContext.Shippers.SingleOrDefault(x => x.Email.Trim() == login.Email);

                    if (shipper == null)
                    {
                        _notyfService.Error("Tài khoản không tồn tại!");
                        return PartialView(login);
                    }

                    string hashedPassword = login.MatKhau.ToMD5();
                    if (shipper.MatKhau != hashedPassword)
                    {
                        _notyfService.Error("Sai thông tin đăng nhập!");
                        return PartialView(login);
                    }

                    HttpContext.Session.SetString("MaShip", shipper.MaShip.ToString());

                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, shipper.Email), 
                new Claim("MaShip", shipper.MaShip.ToString()),
                new Claim(ClaimTypes.Role, "Shipper") 
            };

                    var claimsIdentity = new ClaimsIdentity(claims, "ShipperScheme");
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync("ShipperScheme", claimsPrincipal);

                    _notyfService.Success("Đã đăng nhập thành công!");

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home"); 
                    }
                }
            }
            catch (Exception ex)
            {
                _notyfService.Error("Đã xảy ra lỗi trong quá trình đăng nhập!");
                Console.WriteLine(ex.Message);
                return RedirectToAction("DangKy", "TaiKhoan");
            }

            return PartialView(login);
        }

        [HttpGet]
        public IActionResult DangXuat()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("MaShip");
            _notyfService.Error("Bạn đã đăng xuất");
            return RedirectToAction("Login", "DangNhap");
        }
        [HttpGet]
        public IActionResult QuenMatKhau()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> QuenMatKhau(QuenMatKhauViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _dataContext.Shippers.SingleOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    _notyfService.Error("Email của bạn không tồn tại");
                    return PartialView();
                }

                user.ResetToken = Guid.NewGuid().ToString();
                user.ResetTokenExpiry = DateTime.Now.AddHours(1);
                await _dataContext.SaveChangesAsync();

                var callbackUrl = Url.Action("ResetMatKhau", "TaiKhoan", new { token = user.ResetToken }, protocol: Request.Scheme);
                var message = $"Vui lòng đặt lại mật khẩu của bạn bằng cách <a href='{callbackUrl}'>nhấp vào đây</a>";

                await _emailService.SendEmailAsync(model.Email, "Đặt lại mật khẩu", message);



                return PartialView("XacThuc");
            }

            return View(model);
        }
        [HttpGet]
        public IActionResult ResetMatKhau(string token)
        {
            var model = new ResetMatKhauViewModel { Token = token };
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetMatKhau(ResetMatKhauViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _dataContext.Shippers.SingleOrDefaultAsync(u => u.ResetToken == model.Token && u.ResetTokenExpiry > DateTime.Now);
                if (user == null)
                {
                    _notyfService.Error("Token của bạn đã hết hạn!");
                    return View(model);
                }

                user.MatKhau = model.MatKhauMoi.Trim().ToMD5();
                user.ResetToken = null;
                user.ResetTokenExpiry = null;
                await _dataContext.SaveChangesAsync();

                _notyfService.Success("Mật khẩu của bạn đã đặt thành công!");
                return PartialView("ResetMatKhau");
            }

            return PartialView(model);
        }

    }
}
