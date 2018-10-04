using KalosfideAPI.Data;
using KalosfideAPI.Partages.KeyString;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Clients
{
    public class ClientService : KeyRIdService<Client>, IClientService
    {
        public ClientService(ApplicationContext context, DbSet<Client> dbSet) : base(context, dbSet)
        {
        }
    }
}
