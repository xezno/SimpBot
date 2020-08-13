using Disco;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpBot.Commands
{
    public class Keywords : Command
    {
        public override string Icon => "🔧";

        public override string[] Aliases => new[] { "keywords", "kw" };

        public override string Description => "Test gif keyword parsing";

        public override string[] Syntax => new[] { "message contents" };

        public override int MinArgs => 1;

        public override int MaxArgs => int.MaxValue;

        public override void Run(CommandArgs commandArgs)
        {
            var messageContents = string.Join(' ', commandArgs.Args);

            var eb = Disco.Utils.BuildDefaultEmbed();
            eb.WithTitle("Keywords");
            eb.WithDescription(Utils.GetGifKeywords(messageContents));

            commandArgs.Message.Channel.SendMessageAsync(embed: eb.Build());
        }
    }
}
