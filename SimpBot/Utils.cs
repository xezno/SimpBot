using Catalyst;
using Disco;
using Discord.Rest;
using Discord.WebSocket;
using Mosaik.Core;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimpBot
{
    public class Utils
    {
        private static HttpClient client;
        private static Pipeline pipeline;

        public static string NumberToEmoji(int value)
        {
            var numbers = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            if (value == 10)
                return ":one: :zero:";
            return ":" + numbers[value] + ":";
        }

        /// <summary>
        /// Returns a progress bar generated from progressValue.
        /// </summary>
        /// <param name="progressValue">A float from 0 to 1 containing the current percentage value that should be filled.</param>
        /// <returns>A string containing a progress bar represented by emojis.</returns>
        public static string ProgressBar(float progressValue)
        {
            if (progressValue < 0 || progressValue > 1) throw new Exception("progressValue was not between 0 and 1");
            string generatedStr = "";
            for (float i = 0; i < 1; i += 0.2f)
            {
                if (progressValue > i)
                {
                    generatedStr += "⬛";
                }
                else
                {
                    generatedStr += "⬜";
                }
            }
            return generatedStr;
        }

        public static Pipeline GetPipeline()
        { 
            if (pipeline == null)
            {
                Storage.Current = new OnlineRepositoryStorage(new DiskStorage("catalyst-models"));
                pipeline = Pipeline.For(Language.English);
            }
            return pipeline;
        }


        public static HttpClient GetHttpClient()
        {
            if (client == null)
                client = new HttpClient();
            return client;
        }

        /// <returns>Title, url</returns>
        public static RSG.Promise<(string, string)> GetGif(string searchQuery)
        {
            var promise = new RSG.Promise<(string, string)>();
            var requestLimit = 5;
            var queryUrl = $"https://api.tenor.com/v1/search?q={searchQuery}&key={ConfigBucket.tenorToken}&limit={requestLimit}";
            var query = GetHttpClient().GetAsync(queryUrl).Result;
            var result = query.Content.ReadAsStringAsync().Result;
            var queryObject = Newtonsoft.Json.JsonConvert.DeserializeObject<TenorRequest>(result);

            var randomChoice = new Random().Next(0, queryObject.Results.Length - 1);

            var chosenGif = queryObject.Results[randomChoice];

            promise.Resolve((chosenGif.Title, chosenGif.Url.ToString()));
            return promise;
        }

        public static string GetGifKeywords(string message)
        {
            var nlp = GetPipeline();
            var doc = new Document(message, Language.English);
            nlp.ProcessSingle(doc);

            var keywordStrBuild = new StringBuilder();
            foreach (var token in doc.TokensData)
            {
                foreach (var tokenData in token)
                {
                    if (tokenData.Tag != PartOfSpeech.CCONJ)
                    {
                        keywordStrBuild.Append($"{message.Substring(tokenData.LowerBound, tokenData.UpperBound + 1 - tokenData.LowerBound)} ");
                    }
                }
            }

            return keywordStrBuild.ToString();
        }

        public static RestUserMessage SendPlaceholderMessage(ISocketMessageChannel channel)
        {
            var randomPlaceholder = Placeholders.Instance.Data[new Random().Next(0, Placeholders.Instance.Data.Count - 1)];
            
            return channel.SendMessageAsync($"<a:loading:742933366906552370> {randomPlaceholder}...").Result;
        }
    }
}
