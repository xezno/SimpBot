using Disco;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace SimpBot.Commands
{
    public class Ping : Command
    {
        public override string[] Aliases => new string[] { "ping", "png" };
        public override string[] Syntax => new string[] { };
        public override string Description => "Returns a test ping.";
        public override string Icon => "📶";
        public override int MinArgs => 0;
        public override int MaxArgs => 0;

        public override void Run(CommandArgs args)
        {
            var socketChannel = args.Message.Channel as SocketGuildChannel;
            args.Message.Channel.SendMessageAsync($"{Icon} Pong | {args.DiscordClient.Latency}ms");
        }
    }
}
