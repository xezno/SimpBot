using Newtonsoft.Json;
using System.Collections.Generic;

namespace Disco
{
    public class Ban
    {
        [JsonProperty("server", NullValueHandling = NullValueHandling.Ignore)]
        public string Server { get; set; }

        [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
        public string User { get; set; }

        [JsonProperty("reason", NullValueHandling = NullValueHandling.Ignore)]
        public string Reason { get; set; }
    }

    public class Bans
    {
        public List<Ban> BanList { get; set; }
    }

    public class BanBucket : DataStructure<Bans, BanBucket>
    {
        public override bool ReadOnly => false;
        public override string Location => "Content/Bans.json";

        public static List<Ban> banList { get => Instance.Data.BanList ; set => Instance.Data.BanList = value; }
    }
}