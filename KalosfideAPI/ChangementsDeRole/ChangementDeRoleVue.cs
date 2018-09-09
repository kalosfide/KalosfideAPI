using KalosfideAPI.Data;
using System;

namespace KalosfideAPI.ChangementsDeRole
{
    public class ChangementDeRoleVue
    {
        public int No { get; set; }

        public string UtilisateurId { get; set; }
        public int RoleNo { get; set; }

        public Role Role { get; set; }

        public DateTime Date { get; set; }

        public string Etat { get; set; }
    }
}
