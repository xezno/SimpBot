using Disco;
using Discord;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimpBot.Commands
{
    public class Respond : Command
    {
        public override string[] Aliases => new string[] { "respond", "res", "r" };
        public override string[] Syntax => new string[] { };
        public override string Description => "Responds to your message like a human would.";
        public override string Icon => "🤖";
        public override int MinArgs => 0;
        public override int MaxArgs => int.MaxValue;

        private DateTime lastMessageTime;

        // TODO: Move / merge with Gpt2Bridge
        public override void Run(CommandArgs args)
        {
            if ((DateTime.Now - lastMessageTime).TotalSeconds < ConfigBucket.respondTimeLimit)
            {
                Disco.Utils.SendError(args.Message.Channel, $"Sorry! I can only respond every {ConfigBucket.respondTimeLimit}s. please wait a while before sending the next one. ({ConfigBucket.respondTimeLimit - (DateTime.Now - lastMessageTime).TotalSeconds}s remaining)");
                return;
            }

            lastMessageTime = DateTime.Now;

            // Send random placeholder message
            var placeholderMessage = Utils.SendPlaceholderMessage(args.Message.Channel);

            // Set typing state until we return
            using var typingState = args.Message.Channel.EnterTypingState();

            Gpt2Bridge.GetResponseAsync(args).Then(text => {
                // Select only the message we need
                var splitMessages = text.Split('\n');
                foreach (var userMessage in splitMessages)
                {
                    if (!userMessage.EndsWith(":") && !string.IsNullOrWhiteSpace(userMessage))
                    {
                        // TODO: Multi-line response messages?
                        // Add the original message - show it as a quote

                        Random random = new Random();
                        var pendingMessage = userMessage;

                        // Gif test: get the gif keywords, use it to search, and display at random
                        var gifKeywords = Utils.GetGifKeywords(pendingMessage);
                        if (new Random().Next(0, 10) == 2 && gifKeywords.Length > 0)
                        {
                            Utils.GetGif(gifKeywords).Then((gifResult) =>
                            {
                                Disco.Utils.Log($"Gif opportunity; using keywords {gifKeywords}");
                                pendingMessage = $"||{pendingMessage}||\n{gifResult.Item2}";

                                typingState.Dispose();
                                placeholderMessage.ModifyAsync(m => m.Content = $"> {args.Message.Content}\n{pendingMessage}");
                            });
                        }
                        else
                        {
                            typingState.Dispose();
                            placeholderMessage.ModifyAsync(m => m.Content = $"> {args.Message.Content}\n{pendingMessage}");
                        }
                        break;
                    }
                }
            }).Catch(ex => {
                placeholderMessage.ModifyAsync(m => m.Content = $"⚠ {ex.Message}");
            });
        }
    }
}
