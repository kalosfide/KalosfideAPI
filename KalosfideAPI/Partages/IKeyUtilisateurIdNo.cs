namespace KalosfideAPI.Partages
{
    public interface IKeyUtilisateurIdNo : IKeyUtilisateurId
    {
        int No { get; set; }
    }
    public class KeyUtilisateurIdNo : IKeyUtilisateurIdNo
    {
        public string UtilisateurId { get; set; }
        public int No { get; set; }
        bool Egale(IKeyUtilisateurIdNo key)
        {
            return UtilisateurId == key.UtilisateurId && No == key.No;
        }
    }
    public static class KeyUtilisateurIdNoFabrique
    {
        public static KeyUtilisateurIdNo CréeKey(IKeyUtilisateurIdNo key)
        {
            return new KeyUtilisateurIdNo
            {
                UtilisateurId = key.UtilisateurId,
                No = key.No
            };
        }
        public static IKeyUtilisateurId CréeKey(string param)
        {
            var t = param.Split('/');
            if (t.Length == 1)
            {
                return new KeyUtilisateurId
                {
                    UtilisateurId = param
                };
            }
            if (t.Length != 2)
            {
                return null;
            }
            return new KeyUtilisateurIdNo
            {
                UtilisateurId = t[0],
                No = int.Parse(t[1])
            };
        }
    }
}
