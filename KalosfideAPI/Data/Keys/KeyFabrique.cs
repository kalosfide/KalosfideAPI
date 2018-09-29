using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Data.Keys
{
    public class KeyFabrique
    {
        public KeyUId KeyUId { get; private set; }
        public KeyUIdRNo KeyUIdRNo { get; private set; }
        public KeyUIdRNoNo KeyUIdRNoNo { get; private set; }

        public KeyFabrique(string param)
        {
            if (param != null)
            {
                try
                {
                    var t = param.Split(AKeyString.Séparateur);
                    switch (t.Length)
                    {
                        case 1:
                            KeyUId = new KeyUId
                            {
                                UtilisateurId = t[0],
                            };
                            break;
                        case 2:
                            KeyUIdRNo = new KeyUIdRNo
                            {
                                UtilisateurId = t[0],
                                RoleNo = int.Parse(t[1])
                            };
                            break;
                        case 3:
                            KeyUIdRNoNo = new KeyUIdRNoNo
                            {
                                UtilisateurId = t[0],
                                RoleNo = int.Parse(t[1]),
                                No = long.Parse(t[2])
                            };
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {
                    throw new ArgumentException("Mausaise key");
                }
            }
        }
    }    
}
