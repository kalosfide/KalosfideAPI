using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KalosfideAPI.Data.Keys;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace KalosfideAPI.Fournisseurs
{
    public class FournisseurVue : AKeyRId
    {
        public override string RoleId { get; set; }
    }
}