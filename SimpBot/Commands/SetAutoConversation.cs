using Disco;
using Discord;

namespace SimpBot.Commands
{
    public class SetAutoConversation : Command
    {
        public override string[] Aliases => new string[] { "setAutoConversation", "conversation", "c" };
        public override string[] Syntax => new string[] { "true/false" };
        public override string Description => "Get/set whether the bot is allowed to auto-respond to messages.";
        public override string Icon => "🤖";
        public override int MinArgs => 0;
        public override int MaxArgs => 1;

        public override void Run(CommandArgs args)
        {
            if (args.Args.Length == 0)
            {
                // Get
                EmbedBuilder eb = Disco.Utils.BuildDefaultEmbed();
                eb.WithTitle($"Current auto-respond setting");
                eb.WithDescription($"Auto-respond is {(ConfigBucket.autoRespond ? "enabled" : "disabled")}");
                args.Message.Channel.SendMessageAsync("", false, eb.Build());
            }
            else
            {
                // Set
                if (!bool.TryParse(args.Args[0], out var newAutoRespond))
                {
                    Disco.Utils.SendError(args.Message.Channel, "Please enter a boolean.");
                    return;
                }

                EmbedBuilder eb = Disco.Utils.BuildDefaultEmbed();
                eb.WithTitle($"Changed auto-respond setting");
                eb.WithDescription($"Auto-respond is now {(newAutoRespond ? "enabled" : "disabled")}");
                ConfigBucket.autoRespond = newAutoRespond;
                args.Message.Channel.SendMessageAsync("", false, eb.Build());
            }
        }
    }
}
