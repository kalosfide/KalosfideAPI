using KalosfideAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KalosfideAPI.Sécurité
{
    [Route("api/motdepasse")]
    public class OptionsMotDePasseController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public OptionsMotDePasseController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult GetOptions()
        {
            var options = _userManager.Options.Password;
            var règles = new RèglesDeMotDePasse
            {
                NoSpaces = true,
                RequireDigit = options.RequireDigit,
                RequiredLength = options.RequiredLength,
                RequireLowercase = options.RequireLowercase,
                RequireUppercase = options.RequireUppercase,
                RequireNonAlphanumeric = options.RequireNonAlphanumeric
            };
            return Ok(règles);
        }
    }
}
