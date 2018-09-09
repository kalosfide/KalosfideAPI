using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages
{
    public abstract class SaveChangesController : Controller
    {

        public IActionResult SaveChangesActionResult(BaseServiceRetour retour)
        {
            switch (retour.Type)
            {
                case BaseServiceRetourType.Ok:
                    return NoContent();
                case BaseServiceRetourType.IdentityError:
                    return StatusCode(500, "La création du compte utilisateur a échoué.");
                case BaseServiceRetourType.ConcurrencyError:
                    return StatusCode(409);
                case BaseServiceRetourType.Indéterminé:
                    return StatusCode(500, "Erreur interne inconnue.");
                default:
                    break;
            }
            return StatusCode(500, "Erreur interne inconnue.");
        }
    }
}
