using KalosfideAPI.Data;
using KalosfideAPI.Partages.KeyString;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Administrateurs
{
    public class AdministrateurService : KeyRIdService<Administrateur>, IAdministrateurService
    {
        public AdministrateurService(ApplicationContext context, DbSet<Administrateur> dbSet) : base(context, dbSet)
        {
        }
    }
}
