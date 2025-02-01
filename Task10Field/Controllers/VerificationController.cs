using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task10Field.Data;
using Task10Field.Interfaces;
using Task10Field.Models.Verification;

namespace Task10Field.Controllers
{
    public class VerificationController : Controller
    {
        public readonly ApplicationDbContext _db;
        public readonly IOTPService _otpService;

        public VerificationController(ApplicationDbContext db,IOTPService oTPService)
        {
            _db = db;
            _otpService = oTPService;   
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [HttpGet]
        public IActionResult RequestOTP()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GenerateOTP(string mobileNumber)
        {
            var otp = _otpService.GenerateOTP();

            // Save OTP in database
            var otpEntry = new OTPVerification
            {
                MobileNumber = mobileNumber,
                OTP = otp,
                GeneratedAt = DateTime.UtcNow,
                IsVerified = false
            };

            _db.OTPVerifications.Add(otpEntry);
            await _db.SaveChangesAsync();

            // Send OTP via Twilio
            bool isSent = await _otpService.SendOTPAsync(mobileNumber, otp);

            if (isSent)
            {
                return RedirectToAction("VerifyOTP", new { mobileNumber });
            }
            else
            {
                ModelState.AddModelError("", "Failed to send OTP. Please try again.");
                return View("RequestOTP");
            }
        }


        [HttpGet]
        public IActionResult VerifyOTP(string mobileNumber)
        {
            ViewBag.MobileNumber = mobileNumber;
            return View();
        }
        public async Task<IActionResult> VerifyOTP(string mobileNumber, string otp)
        {
            var otpEntry = await _db.OTPVerifications
                .FirstOrDefaultAsync(o => o.MobileNumber == mobileNumber && o.OTP == otp);

            if (otpEntry != null && otpEntry.GeneratedAt.AddMinutes(5) > DateTime.UtcNow)
            {
                otpEntry.IsVerified = true;
                await _db.SaveChangesAsync();

                return RedirectToAction("Success");
            }

            ModelState.AddModelError("", "Invalid or expired OTP.");
            ViewBag.MobileNumber = mobileNumber;
            return View();
        }
        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }

    }
}
