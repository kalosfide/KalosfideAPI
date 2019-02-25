using KalosfideAPI.Data;
using KalosfideAPI.Data.Constantes;
using KalosfideAPI.Partages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Roles
{
    class GèreEtat : Partages.KeyParams.GéreEtat<Role, RoleVue, EtatRole>
    {
        public GèreEtat(DbSet<EtatRole> dbSetEtat) : base(dbSetEtat)
        { }
        protected override EtatRole CréeEtatAjout(Role donnée)
        {
            EtatRole etat = new EtatRole
            {
                Etat = TypeEtatRole.Nouveau,
                Date = DateTime.Now
            };
            return etat;
        }
        protected override EtatRole CréeEtatEdite(Role donnée, RoleVue vue)
        {
            bool modifié = false;
            EtatRole état = new EtatRole
            {
                Date = DateTime.Now
            };
            if (vue.Etat != null && donnée.Etat != vue.Etat)
            {
                donnée.Etat = vue.Etat;
                état.Etat = vue.Etat;
                modifié = true;
            }
            return modifié ? état : null;
        }
    }

    public class RoleService : Partages.KeyParams.KeyUidRnoService<Role, RoleVue, EtatRole>, IRoleService
    {

        public RoleService(ApplicationContext context) : base(context)
        {
            _dbSet = _context.Role;
            _géreEtat = new GèreEtat(_context.EtatRole);
            _inclutRelations = Complète;
        }

        IQueryable<Role> Complète(IQueryable<Role> données)
        {
            return données.Include(d => d.Site);
        }

        public async Task<RetourDeService<Role>> ChangeEtat(Role role, string état)
        {
            role.Etat = état;
            _context.Role.Update(role);
            EtatRole etatRole = new EtatRole
            {
                Date = DateTime.Now,
                Etat = état
            };
            etatRole.CopieKey(role.KeyParam);
            _context.EtatRole.Add(etatRole);
            return await SaveChangesAsync(role);
        }

        public override Role NouvelleDonnée()
        {
            return new Role();
        }

        public async Task<Role> CréeRole(Utilisateur utilisateur)
        {
            int roleNo = await DernierNo(utilisateur.Uid) + 1;
            Role role = new Role
            {
                Uid = utilisateur.Uid,
                Rno = roleNo,
                Etat = TypeEtatRole.Nouveau
            };
            return role;
        }

        public override RoleVue CréeVue(Role donnée)
        {
            RoleVue vue = new RoleVue
            {
                SiteUid = donnée.SiteUid,
                SiteRno = donnée.SiteRno,
                NomSite = donnée.Site.NomSite,
            };
            vue.CopieKey(donnée.KeyParam);
            return vue;
        }

        public override Task CopieVueDansDonnées(Role donnée, RoleVue vue)
        {
            return Task.CompletedTask;
        }
    }
}