
namespace OpenGate.Models
{
    public class LdapManager
    {
        public Dictionary<string, string> ToDictionary(IEnumerable<string> input)
        {
            var dictionary = new Dictionary<string, string>();
            
            foreach (var item in input)
            {
                var split = item.Split('=').ToList();

                var key = split[0];

                split.RemoveAt(0);
                var value = string.Join('=', split);
                dictionary.Add(key, value);
            }

            return dictionary;
        }
    }
}
