using KalosfideAPI.Data;
using KalosfideAPI.Partages;
using System.Threading.Tasks;

namespace KalosfideAPI.Roles
{
    public interface IRoleService : Partages.KeyParams.IKeyUidRnoService<Role, RoleVue>
    {
        Task<Role> CréeRole(Utilisateur utilisateur);
        Task<RetourDeService<Role>> ChangeEtat(Role role, string état);
    }
}
