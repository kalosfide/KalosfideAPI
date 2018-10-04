using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KalosfideAPI.Data;
using KalosfideAPI.Data.Constantes;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Enregistrement;
using Microsoft.EntityFrameworkCore;

namespace KalosfideAPI.Partages.KeyString
{
    public abstract class KeyRIdService<T> : KeyStringService<T>, IKeyRIdService<T> where T : AKeyRId
    {
        public KeyRIdService(ApplicationContext context, DbSet<T> dbSet) : base(context, dbSet)
        {
        }
    }
}
