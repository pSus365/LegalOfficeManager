using LegalOfficeManagerApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class TwoFactorAuthController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly Microsoft.AspNetCore.Identity.UI.Services.IEmailSender _emailSender;

    public TwoFactorAuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
    }

    [HttpGet]
    public async Task<IActionResult> EnableEmail2FA()
    {
        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

        if (user == null)
        {
            return NotFound();
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> EnableEmail2FA(bool enable)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        await _userManager.SetTwoFactorEnabledAsync(user, enable);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> SendVerificationCode()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

        await _emailSender.SendEmailAsync(
            user.Email,
            "Twój kod weryfikacyjny",
            $"Twój kod weryfikacyjny to: {token}");

        return RedirectToAction("VerifyCode");
    }

    [HttpGet]
    public IActionResult VerifyCode()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyCode(string code)
    {
        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user == null) return RedirectToAction("Login", "LogInPage");

        var result = await _signInManager.TwoFactorSignInAsync("Email", code, false, rememberClient: false);

        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "Nieprawidłowy kod weryfikacyjny");
        return View("VerifyCode");
    }


    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> LoginWith2fa()
    {
        var email = TempData["EmailFor2FA"] as string;
        if (string.IsNullOrEmpty(email)) return RedirectToAction("Login", "LogInPage");

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return RedirectToAction("Login", "LogInPage");

        // Wygeneruj token
        var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

        await _emailSender.SendEmailAsync(
            user.Email,
            "Twój kod weryfikacyjny",
            $"Twój kod weryfikacyjny to: {token}");

        TempData["UserIdFor2FA"] = user.Id;
        return View();
    }

}