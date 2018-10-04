using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Data.Keys
{
    public static class KeyFabrique
    {

        private static List<AKeyBase> CréeKeys(string param)
        {
            if (param != null)
            {
                try
                {
                    var t = param.Split(AKeyBase.Séparateur);
                    int roleNo;
                    switch (t.Length)
                    {
                        case 1:
                            return new List<AKeyBase>
                            {
                                new KeyUId
                                {
                                UtilisateurId = t[0],
                                }
                            };
                        case 2:
                            if (int.TryParse(t[1], out roleNo))
                            {
                                return new List<AKeyBase>
                                {
                                    new KeyUIdRNo
                                    {
                                        UtilisateurId = t[0],
                                        RoleNo = int.Parse(t[1])
                                    },
                                    new KeyRId
                                    {
                                        RoleId = param
                                    }
                                };
                            }
                            break;
                        case 3:
                            if (int.TryParse(t[1], out roleNo))
                            {
                                if (long.TryParse(t[2], out long no))
                                {
                                    return new List<AKeyBase>
                                    {
                                        new KeyRIdNo
                                        {
                                            RoleId = t[0] + AKeyBase.Séparateur + roleNo,
                                            No = no
                                        }
                                    };
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {
                    // throw new ArgumentException("Mausaise key");
                }
            }
            return null;
        }

        public static KeyUId CréeKeyUId(string texteKey)
        {
            var keys = CréeKeys(texteKey);
            if (keys != null)
            {
                if (keys.Count == 1 && keys.ElementAt(0) is KeyUId)
                {
                    return keys.ElementAt(0) as KeyUId;
                }
            }
            return null;
        }

        public static KeyUIdRNo CréeKeyUIdRNo(string texteKey)
        {
            var keys = CréeKeys(texteKey);
            if (keys != null)
            {
                if (keys.Count == 2 && keys.ElementAt(0) is KeyUIdRNo)
                {
                    return keys.ElementAt(0) as KeyUIdRNo;
                }
            }
            return null;
        }

        public static AKeyBase CréeKeyUIdOuUIdRNo(string texteKey)
        {
            var keys = CréeKeys(texteKey);
            if (keys != null)
            {
                if ((keys.Count == 1 && keys.ElementAt(0) is KeyUId) || (keys.Count == 2 && keys.ElementAt(0) is KeyUIdRNo))
                {
                    return keys.ElementAt(0);
                }
            }
            return null;
        }

        public static KeyRId CréeKeyRId(string texteKey)
        {
            var keys = CréeKeys(texteKey);
            if (keys != null)
            {
                if (keys.Count == 2 && keys.ElementAt(1) is KeyRId)
                {
                    return keys.ElementAt(1) as KeyRId;
                }
            }
            return null;
        }

        public static KeyRIdNo CréeKeyRIdNo(string texteKey)
        {
            var keys = CréeKeys(texteKey);
            if (keys != null)
            {
                if (keys.Count == 1 && keys.ElementAt(0) is KeyRIdNo)
                {
                    return keys.ElementAt(0) as KeyRIdNo;
                }
            }
            return null;
        }

        public static AKeyBase CréeKeyRIdOuRIdNo(string texteKey)
        {
            var keys = CréeKeys(texteKey);
            if (keys != null)
            {
                if ((keys.Count == 2 && keys.ElementAt(1) is KeyRId) || (keys.Count == 1 && keys.ElementAt(0) is KeyRIdNo))
                {
                    return keys.ElementAt(0);
                }
            }
            return null;
        }

        public static string CréeTexteKey(params string[] segments)
        {
            return string.Join(AKeyBase.Séparateur, segments);
        }
    }    
}
