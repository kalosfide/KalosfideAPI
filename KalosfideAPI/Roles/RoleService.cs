using KalosfideAPI.Data;
using KalosfideAPI.Data.Enums;
using KalosfideAPI.Partages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Roles
{
    public class RoleService : Partages.KeyString.KeyUIdRNoService<Role>, IRoleService
    {

        public RoleService(ApplicationContext context) : base(context, context.Role)
        {
        }

        public async Task<List<Role>> Fournisseurs()
        {
            return await _context.Role.Where(role => role.Type == TypeDeRole.Fournisseur.Code).ToListAsync();
        }

        public new async Task<RetourDeService<Role>> Ajoute(Role role)
        {
            RetourDeService<Role> retour = await base.Ajoute(role);
            if (retour.Ok)
            {
                retour = await ChangeEtat(role, EtatRole.Nouveau);
            }
            return retour;
        }

        public async Task<RetourDeService<Role>> ChangeEtat(Role role, string état)
        {
            ChangementEtatRole etatDeRole = new ChangementEtatRole
            {
                UtilisateurId = role.UtilisateurId,
                RoleNo = role.RoleNo,
                Date = DateTime.Now,
                Etat = état
            };
            _context.JournalEtatRole.Add(etatDeRole);
            role.Etat = état;
            try
            {
                await _context.SaveChangesAsync();
                return new RetourDeService<Role>(role);
            }
            catch (DbUpdateConcurrencyException)
            {
                return new RetourDeService<Role>(TypeRetourDeService.ConcurrencyError);
            }
            catch (Exception)
            {
                return new RetourDeService<Role>(TypeRetourDeService.Indéterminé);
            }
        }
    }
}