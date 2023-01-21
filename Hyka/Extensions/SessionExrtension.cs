using Hyka.Dtos;
using Hyka.Models;
using Newtonsoft.Json;

namespace Hyka.Extensions
{
    public static class SessionExtension
    {

        public static void SetObject<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            // Is redundant, but prevents session variables from being null.
            if (value == null)
            {
                session._setSessionVaribles();
                value = session.GetString(key);
            }
            return JsonConvert.DeserializeObject<T>(value);
        }

        private static void _setSessionVaribles(this ISession session)
        {
            session.SetObject("INGRESS_LIST", new List<KeyValuePair<PersonDto, Tariff>>());
            session.SetObject("TOTAL", 0);
        }
    }
}