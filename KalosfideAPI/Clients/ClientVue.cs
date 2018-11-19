using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KalosfideAPI.Data.Keys;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace KalosfideAPI.Clients
{
    public class ClientVue : AKeyUidRno
    {
        public override string Uid { get; set; }
        public override int Rno { get; set; }
        public string Nom { get; set; }
        public string Adresse { get; set; }
    }
}