using System.Collections.Generic;

namespace Idera.SQLsecure.UI.Console.SQL
{
    public class Tag
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }
        public bool IsDefault { get; set; }
        public List<TaggedServer> TaggedServers { get; set; }

        public Tag()
        {
            TaggedServers = new List<TaggedServer>();
        }

    }
}