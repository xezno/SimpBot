using Disco;
using Discord;
using System.Threading.Tasks;

namespace SimpBot.Commands
{
    public class SetContext : Command
    {
        public override string[] Aliases => new string[] { "setContext", "context", "con" };
        public override string[] Syntax => new string[] { "message count" };
        public override string Description => "Get/set how many messages the bot receives as context.";
        public override string Icon => "🤖";
        public override int MinArgs => 0;
        public override int MaxArgs => 1;

        public override void Run(CommandArgs args)
        {
            if (args.Args.Length == 0)
            {
                // Get
                EmbedBuilder eb = Disco.Utils.BuildDefaultEmbed();
                eb.WithTitle($"Current context");
                eb.WithDescription($"S.I.M.P. is currently using {ConfigBucket.context} messages as context.");
            }
            else
            {
                // Set
                if (!int.TryParse(args.Args[0], out var newContext))
                {
                    Disco.Utils.SendError(args.Message.Channel, "Please enter a number.");
                    return;
                }
                if (newContext < 1 || newContext > 25)
                {
                    Disco.Utils.SendError(args.Message.Channel, "Please choose a number between 1 and 25.");
                    return;
                }

                EmbedBuilder eb = Disco.Utils.BuildDefaultEmbed();
                eb.WithTitle($"Changed context");
                eb.WithDescription($"Now using {newContext} messages as context.");
                ConfigBucket.context = newContext;
            }
        }
    }
}
