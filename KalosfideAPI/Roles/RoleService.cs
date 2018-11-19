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
    public class RoleService : Partages.KeyParams.KeyUidRnoService<Role>, IRoleService
    {

        public RoleService(ApplicationContext context) : base(context)
        {
            _dbSet = _context.Role;
        }

        public new void AjouteSansSauver(Role role)
        {
            base.AjouteSansSauver(role);
            _ChangeEtat(role, TypeEtatRole.Nouveau);
        }

        public void _ChangeEtat(Role role, string état)
        {
            EtatRole etatDeRole = new EtatRole
            {
                Uid = role.Uid,
                Rno = role.Rno,
                Date = DateTime.Now,
                Etat = état
            };
            _context.EtatRole.Add(etatDeRole);
        }

        public async Task<RetourDeService<Role>> ChangeEtat(Role role, string état)
        {
            _ChangeEtat(role, état);
            return await SaveChangesAsync(role);
        }
    }
}