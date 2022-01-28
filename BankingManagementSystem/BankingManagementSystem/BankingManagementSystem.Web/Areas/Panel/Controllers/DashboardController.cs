using BankingManagementSystem.Membership.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BankingManagementSystem.Web.Areas.Panel.Controllers
{
    [Area("Panel"),Authorize]
    public class DashboardController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<DashboardController> _logger;


        public DashboardController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<DashboardController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CheckBalance()
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser user = await _userManager.GetUserAsync(User);
            TempData["Balance"] = $"Your Balance : {user.Balance}";
            return View();
        }

        public async Task<IActionResult> AddBalance()
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser user = await _userManager.GetUserAsync(User);

            Model2 model2 = new Model2();

            return View(model2);
        }

        [HttpPost, AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddBalance(Model2 model2)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser user = await _userManager.GetUserAsync(User);
            user.Balance  += (Convert.ToInt32(model2?.Balance));
            await _userManager.UpdateAsync(user);
            return RedirectToAction(nameof(CheckBalance));
        }

        public async Task<IActionResult> WithdrawBalances()
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser user = await _userManager.GetUserAsync(User);

            Model2 model2 = new Model2();

            return View(model2);
        }

        [HttpPost, AutoValidateAntiforgeryToken]
        public async Task<IActionResult> WithdrawBalances(Model2 model2)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user.Balance>=int.Parse(model2.Balance))
            {
                user.Balance -= (Convert.ToInt32(model2?.Balance));
                await _userManager.UpdateAsync(user);
            }
            else
            {
                TempData["failed"] = $"you dont have enough balance";
            }

            return RedirectToAction(nameof(CheckBalance));
        }

        public async Task<IActionResult> TransferBalance()
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser user = await _userManager.GetUserAsync(User);

            Model2 model2 = new Model2();

            return View(model2);
        }

        [HttpPost, AutoValidateAntiforgeryToken]
        public async Task<IActionResult> TransferBalance(Model2 model2)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser user = await _userManager.GetUserAsync(User);

            if (user.Balance >= int.Parse(model2.Balance))
            {
                user.Balance -= (Convert.ToInt32(model2?.Balance));

                await _userManager.UpdateAsync(user);

                var transferUser = await _userManager.FindByEmailAsync(model2.Email);
                transferUser.Balance += (Convert.ToInt32(model2?.Balance));
                await _userManager.UpdateAsync(transferUser);
                TempData["Transfer"] = $"Transfer : {model2.Balance} successfully";
            }
            else
            {
                TempData["failed"] = $"you dont have enough balance";
            }

            return RedirectToAction(nameof(CheckBalance));
        }


    }
}
