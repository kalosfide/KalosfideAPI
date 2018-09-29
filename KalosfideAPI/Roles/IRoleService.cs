using KalosfideAPI.Data;
using KalosfideAPI.Partages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KalosfideAPI.Roles
{
    public interface IRoleService : Partages.KeyString.IKeyUIdRNoService<Role>
    {
        Task<List<Role>> Fournisseurs();
        Task<RetourDeService<Role>> ChangeEtat(Role role, string état);
    }
}
