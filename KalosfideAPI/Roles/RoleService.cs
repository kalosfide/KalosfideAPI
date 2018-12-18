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
    public class RoleService : Partages.KeyParams.KeyUidRnoService<Role, RoleVue>, IRoleService
    {

        public RoleService(ApplicationContext context) : base(context)
        {
            _dbSet = _context.Role;
            _inclutRelations = Complète;
        }

        IQueryable<Role> Complète(IQueryable<Role> données)
        {
            return données.Include(d => d.Site);
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

        public override Role NouvelleDonnée()
        {
            return new Role();
        }

        public override RoleVue CréeVue(Role donnée)
        {
            RoleVue vue = new RoleVue
            {
                SiteUid = donnée.SiteUid,
                SiteRno = donnée.SiteRno,
                NomSite = donnée.Site.NomSite,
            };
            FixeVueKey(donnée, vue);
            return vue;
        }

        public override Task CopieVueDansDonnées(Role donnée, RoleVue vue)
        {
            return Task.CompletedTask;
        }
    }
}