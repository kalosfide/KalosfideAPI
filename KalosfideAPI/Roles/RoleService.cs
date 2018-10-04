using KalosfideAPI.Data;
using KalosfideAPI.Data.Constantes;
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

        public new void AjouteSansSauver(Role role)
        {
            base.AjouteSansSauver(role);
            _ChangeEtat(role, EtatRole.Nouveau);
        }

        public void _ChangeEtat(Role role, string état)
        {
            ChangementEtatRole etatDeRole = new ChangementEtatRole
            {
                RoleId = role.RoleId,
                Date = DateTime.Now,
                Etat = état
            };
            _context.JournalEtatRole.Add(etatDeRole);
            role.Etat = état;
        }

        public async Task<RetourDeService<Role>> ChangeEtat(Role role, string état)
        {
            _ChangeEtat(role, état);
            return await SaveChangesAsync(role);
        }
    }
}