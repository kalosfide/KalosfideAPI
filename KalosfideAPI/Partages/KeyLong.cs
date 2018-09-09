using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages
{
    public interface IKeyLong
    {
        long Id { get; set; }
    }
    public class KeyLong : IKeyLong
    {
        public long Id { get; set; }
    }
    public static class KeyLongFabrique
    {
        public static KeyLong CréeKey(IKeyLong key)
        {
            return new KeyLong
            {
                Id = key.Id,
            };
        }
        public static KeyLong CréeKey(string param)
        {
            var t = param.Split('/');
            if (t.Length != 1)
            {
                return null;
            }
            return new KeyLong
            {
                Id = long.Parse(t[0]),
            };
        }
    }
}
