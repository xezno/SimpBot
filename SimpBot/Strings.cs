using Disco;
using System.Collections.Generic;

namespace SimpBot
{
    public class Strings : DataStructure<Dictionary<string, string>, Strings>
    {
        public override bool ReadOnly => false;
        public override string Location => "Content/Users.json";
    }
}
