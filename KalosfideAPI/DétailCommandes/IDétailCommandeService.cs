using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Partages.KeyParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.DétailCommandes
{
    public interface IDétailCommandeService : IKeyParamService<DétailCommande, DétailCommandeVue, KeyParam>
    {
    }
}
