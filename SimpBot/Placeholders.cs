using Disco;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpBot
{
    public class Placeholders : DataStructure<List<string>, Placeholders>
    {
        public override bool ReadOnly => true;

        public override string Location => "Content/Placeholders.json";
    }
}
