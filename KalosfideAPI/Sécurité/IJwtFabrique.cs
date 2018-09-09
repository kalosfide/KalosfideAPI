using KalosfideAPI.Data;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KalosfideAPI.Sécurité
{
    public interface IJwtFabrique
    {
        JwtRéponse CréeReponse(ApplicationUser user, Utilisateur utilisateurAvecRoleSelectionné);
    }
}
