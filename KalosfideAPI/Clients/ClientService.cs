using KalosfideAPI.Data;
using KalosfideAPI.Partages.KeyParams;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Clients
{
    public class ClientService : KeyUidRnoService<Client>, IClientService
    {
        public ClientService(ApplicationContext context) : base(context)
        {
            _dbSet = _context.Client;
        }
    }
}
