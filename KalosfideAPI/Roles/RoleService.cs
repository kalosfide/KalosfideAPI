using KalosfideAPI.Data;
using KalosfideAPI.Partages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace KalosfideAPI.Roles
{
    public class RoleService : KeyUtilisateurIdNoService<Role>, IRoleService
    {

        public RoleService(ApplicationContext context) : base(context, context.Role)
        {
        }

        public new async Task<BaseServiceRetour<Role>> Ajoute(Role role)
        {
            BaseServiceRetour<Role> retour = await base.Ajoute(role);
            if (retour.Ok)
            {
                retour = await ChangeEtat(role, EtatRole.Nouveau);
            }
            return retour;
        }

        public async Task<BaseServiceRetour<Role>> ChangeEtat(Role role, string état)
        {
            ChangementEtatRole etatDeRole = new ChangementEtatRole
            {
                UtilisateurId = role.UtilisateurId,
                RoleNo = role.No,
                Date = DateTime.Now,
                Etat = état
            };
            _context.JournalEtatRole.Add(etatDeRole);
            role.Etat = état;
            try
            {
                await _context.SaveChangesAsync();
                return new BaseServiceRetour<Role>(role);
            }
            catch (DbUpdateConcurrencyException)
            {
                return new BaseServiceRetour<Role>(BaseServiceRetourType.ConcurrencyError);
            }
            catch (Exception)
            {
                return new BaseServiceRetour<Role>(BaseServiceRetourType.Indéterminé);
            }
        }
    }
}