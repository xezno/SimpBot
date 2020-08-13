using Disco;
using Discord;
using RSG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimpBot
{
    public static class Gpt2Bridge
    {
        private static async Task<string> GetMessagePrefix(CommandArgs args)
        {
            var prefix = "";
            var lastAuthor = "";
            var thisMessageContents = string.Join(' ', args.Args);

            var history = AsyncEnumerableExtensions.Flatten(args.Message.Channel.GetMessagesAsync(20));
            var reversedHistory = history.Reverse()
                                         .Where((message) => { return !message.Content.StartsWith("!") && !message.Content.StartsWith(ConfigBucket.prefix); })
                                         .Take(ConfigBucket.context - 1);

            await foreach (var message in reversedHistory)
            {
                var messageContent = message.Content;

                // TODO: Filter out old commands
                if (lastAuthor == message.Author.Username)
                {
                    prefix += $"{messageContent}\n";
                }
                else
                {
                    prefix += $"\n\n{message.Author.Username}-real:\n{messageContent}\n";
                }
                lastAuthor = message.Author.Username;
            }

            // Add current message's contents
            if (lastAuthor == args.Message.Author.Username)
            {
                prefix += $"{thisMessageContents}\n";
            }
            else
            {
                prefix += $"\n\n{args.Message.Author.Username}-real:\n{thisMessageContents}\n";
            }

            // Add custom user to prefix
            prefix += $"\n\n{ConfigBucket.currentPersonality}:";
            return prefix;
        }

        public static Promise<string> GetResponseAsync(CommandArgs args)
        {
            var promise = new Promise<string>();

            var prefix = GetMessagePrefix(args).Result;
            
            var values = new Dictionary<string, string>()
            {
                { "temperature", ConfigBucket.temperature.ToString() },
                { "prefix", prefix },
                { "include_prefix", "false" },
                { "length", "50" },
                { "count", "1" },
                { "top_p", ConfigBucket.topP.ToString() },
                { "top_k", ConfigBucket.topK.ToString() }
            };

            var serializedValues = Newtonsoft.Json.JsonConvert.SerializeObject(values);
            
            prefix = $"{args.Message.Author.Username}\n{prefix}";

            var content = new StringContent(serializedValues, System.Text.Encoding.UTF8, "application/json");
            var request = Utils.GetHttpClient().PostAsync($"{ConfigBucket.apiEndpoint}/", content);
            if (request.IsCompletedSuccessfully)
            {
                var response = request.Result;
                var responseString = response.Content.ReadAsStringAsync().Result;

                // Read as json
                var deserializedResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);
                var text = deserializedResponse["text"];

                promise.Resolve(text);
            }
            else
            {
                promise.Reject(new Exception("Sorry, I can't help with that. :("));
            }

            return promise;
        }
    }
}
