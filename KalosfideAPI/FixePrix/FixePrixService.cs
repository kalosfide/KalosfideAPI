using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Partages;
using KalosfideAPI.Partages.KeyParams;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.FixePrix
{
    public class FixePrixService : KeyUidRnoNoService<EtatPrix, FixePrix>, IFixePrixService
    {
        public FixePrixService(ApplicationContext context) : base(context)
        {
            _dbSet = null;
            _inclutRelations = Complète;
        }

        IQueryable<EtatPrix> Complète(IQueryable<EtatPrix> etats)
        {
            return etats
                .OrderBy(e => e.Date)
                .GroupBy(e => new { e.Uid, e.Rno, e.No })
                .Select(g => g.Last())
                .Include(e => e.Produit);
        }

        public override Task CopieVueDansDonnées(EtatPrix donnée, FixePrix vue)
        {
            donnée.Date = DateTime.Now;
            donnée.Prix = vue.Nouveau;
            return Task.CompletedTask;
        }

        public override EtatPrix NouvelleDonnée()
        {
            return new EtatPrix();
        }

        public override FixePrix CréeVue(EtatPrix donnée)
        {
            FixePrix vue = new FixePrix();
            FixeVueKey(donnée, vue);
            vue.Nom = donnée.Produit.Nom;
            vue.Actuel = donnée.Prix;
            return vue;
        }
    }
}
