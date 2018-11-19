using KalosfideAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages
{
    public abstract class BaseController : Controller
    {

        public IActionResult SaveChangesActionResult(RetourDeService retour)
        {
            switch (retour.Type)
            {
                case TypeRetourDeService.Ok:
                    return NoContent();
                case TypeRetourDeService.IdentityError:
                    return StatusCode(500, "La création du compte utilisateur a échoué.");
                case TypeRetourDeService.ConcurrencyError:
                    return StatusCode(409);
                case TypeRetourDeService.Indéterminé:
                    return StatusCode(500, "Erreur interne inconnue.");
                default:
                    break;
            }
            return StatusCode(500, "Erreur interne inconnue.");
        }
    }
}
