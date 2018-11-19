using KalosfideAPI.Data.Keys;

namespace KalosfideAPI.Roles
{
    public class RoleVue : AKeyUidRno
    {
        public override string Uid { get; set; }
        public override int Rno { get; set; }

        // données
        public string SiteId { get; set; }
    }
}
