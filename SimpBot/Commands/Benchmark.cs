using Disco;
using System;
using System.Threading.Tasks;

namespace SimpBot.Commands
{
    public class Benchmark : Command
    {
        public override string[] Aliases => new string[] { "benchmark", "bench" };
        public override string[] Syntax => new string[] { };
        public override string Description => "Benchmark the bot's response time.";
        public override string Icon => "🤖";
        public override int MinArgs => 0;
        public override int MaxArgs => int.MaxValue;

        public override void Run(CommandArgs args)
        {
            // Set typing state until we return
            using var typingState = args.Message.Channel.EnterTypingState();

            DateTime startTime = DateTime.Now;
            Gpt2Bridge.GetResponseAsync(args).Then(_ => {
                DateTime endTime = DateTime.Now;
                args.Message.Channel.SendMessageAsync($"Response took {(endTime - startTime).TotalSeconds} seconds.");
            });
        }
    }
}
