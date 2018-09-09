using KalosfideAPI.Data;
using KalosfideAPI.Partages;
using System.Threading.Tasks;

namespace KalosfideAPI.Roles
{
    public interface IRoleService : IKeyUtilisateurIdNoService<Role>
    {
        Task<BaseServiceRetour<Role>> ChangeEtat(Role role, string état);
    }
}
