using Disco;
using Discord;
using System.Threading.Tasks;

namespace SimpBot.Commands
{
    public class SetTemperature : Command
    {
        public override string[] Aliases => new string[] { "setTemperature", "temperature", "temp" };
        public override string[] Syntax => new string[] { "temperature value" };
        public override string Description => "Get/set how WaCkY the responses are.";
        public override string Icon => "🤖";
        public override int MinArgs => 0;
        public override int MaxArgs => 1;

        public override void Run(CommandArgs args)
        {
            if (args.Args.Length == 0)
            {
                // Get
                EmbedBuilder eb = Disco.Utils.BuildDefaultEmbed();
                eb.WithTitle($"Current temperature");
                eb.WithDescription($"S.I.M.P. is currently responding with a temperature of {ConfigBucket.temperature}");
                args.Message.Channel.SendMessageAsync("", false, eb.Build());
            }
            else
            {
                // Set
                if (!decimal.TryParse(args.Args[0], out var newTemperature))
                {
                    Disco.Utils.SendError(args.Message.Channel, "Please enter a number.");
                    return;
                }
                if (newTemperature < 0 || newTemperature > int.MaxValue)
                {
                    Disco.Utils.SendError(args.Message.Channel, $"Please choose a number between 0 and {int.MaxValue}.");
                    return;
                }

                EmbedBuilder eb = Disco.Utils.BuildDefaultEmbed();
                eb.WithTitle($"Changed temperature");
                eb.WithDescription($"Now responding with temperature {newTemperature}");
                ConfigBucket.temperature = newTemperature;
                args.Message.Channel.SendMessageAsync("", false, eb.Build());
            }
        }
    }
}
