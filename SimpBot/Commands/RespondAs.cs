using Disco;
using Discord;
using System.Threading.Tasks;

namespace SimpBot.Commands
{
    public class RespondAs : Command
    {
        public override string[] Aliases => new string[] { "respondAs", "become", "changePersonality" };
        public override string[] Syntax => new string[] { "name" };
        public override string Description => "Get/set the bot's personality based on a specific user.";
        public override string Icon => "🧑";
        public override int MinArgs => 0;
        public override int MaxArgs => int.MaxValue;

        public override void Run(CommandArgs args)
        {
            if (args.Args.Length == 0)
            {
                var userName = string.Join(' ', args.Args);
                EmbedBuilder eb = Disco.Utils.BuildDefaultEmbed();
                eb.WithTitle($"Current personality");
                eb.WithDescription($"Currently responding as {ConfigBucket.currentPersonality}");
                args.Message.Channel.SendMessageAsync("", false, eb.Build());
            }
            else
            {
                var userName = string.Join(' ', args.Args);
                EmbedBuilder eb = Disco.Utils.BuildDefaultEmbed();
                eb.WithTitle($"Changed personality");
                eb.WithDescription($"Now responding as {userName}");
                ConfigBucket.currentPersonality = userName;
                args.Message.Channel.SendMessageAsync("", false, eb.Build());
            }
        }
    }
}
