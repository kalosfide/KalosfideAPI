using KalosfideAPI.Data;
using KalosfideAPI.Partages.KeyParams;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Clients
{
    public class ClientService : KeyUidRnoService<Client, ClientVue>, IClientService
    {
        public ClientService(ApplicationContext context) : base(context)
        {
            _dbSet = _context.Client;
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
            FixeVueKey(donnée, vue);
            return vue;
        }
    }
}
