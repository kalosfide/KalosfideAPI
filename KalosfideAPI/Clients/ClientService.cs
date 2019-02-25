using KalosfideAPI.Data;
using KalosfideAPI.Enregistrement;
using KalosfideAPI.Partages.KeyParams;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace KalosfideAPI.Clients
{
    class GèreEtat : Partages.KeyParams.GéreEtat<Client, ClientVue, EtatClient>
    {
        public GèreEtat(DbSet<EtatClient> dbSetEtat) : base(dbSetEtat)
        { }
        protected override EtatClient CréeEtatAjout(Client donnée)
        {
            EtatClient etat = new EtatClient
            {
                Nom = donnée.Nom,
                Adresse = donnée.Adresse,
                Date = DateTime.Now
            };
            return etat;
        }
        protected override EtatClient CréeEtatEdite(Client donnée, ClientVue vue)
        {
            bool modifié = false;
            EtatClient état = new EtatClient
            {
                Date = DateTime.Now
            };
            if (vue.Nom != null && donnée.Nom != vue.Nom)
            {
                donnée.Nom = vue.Nom;
                état.Nom = vue.Nom;
                modifié = true;
            }
            if (vue.Adresse != null && donnée.Adresse != vue.Adresse)
            {
                donnée.Adresse = vue.Adresse;
                état.Adresse = vue.Adresse;
                modifié = true;
            }
            return modifié ? état : null;
        }
    }

    public class ClientService : KeyUidRnoService<Client, ClientVue, EtatClient>, IClientService
    {
        public ClientService(ApplicationContext context) : base(context)
        {
            _dbSet = _context.Client;
            _géreEtat = new GèreEtat(_context.EtatClient);
        }

        public Client CréeClient(Role role, EnregistrementClientVue clientVue)
        {
            Client client = new Client
            {
                Nom = clientVue.Nom,
                Adresse = clientVue.Adresse,
            };
            role.SiteUid = clientVue.SiteUid;
            role.SiteRno = clientVue.SiteRno;
            return client;
        }

        public override Task CopieVueDansDonnées(Client donnée, ClientVue vue)
        {
            donnée.Nom = vue.Nom;
            donnée.Adresse = vue.Adresse;
            return Task.CompletedTask;
        }

        public override Client NouvelleDonnée()
        {
            return new Client();
        }

        public override ClientVue CréeVue(Client donnée)
        {
            ClientVue vue = new ClientVue
            {
                Nom = donnée.Nom,
                Adresse = donnée.Adresse,
            };
            vue.CopieKey(donnée.KeyParam);
            return vue;
        }
    }
}
