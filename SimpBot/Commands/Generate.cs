using Disco;
using Discord;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimpBot.Commands
{
    public class Generate : Command
    {
        public override string[] Aliases => new string[] { "generate", "gen" };
        public override string[] Syntax => new string[] { "prefix" };
        public override string Description => "Generates a conversation based on the given context.";
        public override string Icon => "🤖";
        public override int MinArgs => 1;
        public override int MaxArgs => int.MaxValue;

        public override void Run(CommandArgs args)
        {
            // Set typing state until we return
            using var typingState = args.Message.Channel.EnterTypingState();

            DateTime startTime = DateTime.Now;
            var prefix = string.Join(' ', args.Args);
            prefix = $"{args.Message.Author.Username}:\n{prefix}\n";

            var values = new Dictionary<string, string>
            {
                { "temperature", ConfigBucket.temperature.ToString() },
                { "prefix", prefix },
                { "include_prefix", "true" },
                { "length", "250" },
                { "top_p", ConfigBucket.topP.ToString() },
                { "top_k", ConfigBucket.topK.ToString() }
            };

            var serializedValues = Newtonsoft.Json.JsonConvert.SerializeObject(values);
            
            // Disco.Utils.Log($"Prefix: {prefix}");

            var content = new StringContent(serializedValues, System.Text.Encoding.UTF8, "application/json");
            var response = Utils.GetHttpClient().PostAsync($"{ConfigBucket.apiEndpoint}/", content).Result;
            var responseString = response.Content.ReadAsStringAsync().Result;

            // Read as json
            var deserializedResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);
            var text = deserializedResponse["text"];
            // Replace \ns with new lines
            text.Replace("\\n", "\n");

            DateTime endTime = DateTime.Now;
            EmbedBuilder eb = Disco.Utils.BuildDefaultEmbedWithTime(startTime, endTime);
            eb.WithTitle($"Generated conversation");
            eb.WithDescription($"```{text}```");
            args.Message.Channel.SendMessageAsync("", false, eb.Build());
        }
    }
}
