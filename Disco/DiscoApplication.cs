using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Discord.Webhook;

namespace Disco
{
    public class DiscoApplication
    {
        public DiscordWebhookClient Client { get; private set; }

        private List<User> users = new List<User>();
        private Random random = new Random();
        private TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        private List<string> messageLog = new List<string>() { "xezno:\nwhats up losers\n\n" };

        public async Task Run()
        {
            Console.WriteLine("Starting webhook bot...");
            Client = new DiscordWebhookClient(ConfigBucket.webhookUrl);
            Console.WriteLine("Finished initialization.");

            GenerateUsers();
            MainLoop();

            await Task.Delay(-1);
        }

        private void GenerateUsers()
        {
            for (int i = 0; i < 10; ++i)
            {
                users.Add(GenerateUser());
            }
        }

        private void MainLoop()
        {
            while (true)
            {
                var user = users[random.Next(0, users.Count)];
                Client.SendMessageAsync(GenerateMessage(user), username: user.Name, avatarUrl: user.Avatar);
                System.Threading.Thread.Sleep(500);
            }
        }

        private User GenerateUser()
        {
            var gender = new Random().Next(0, 2) == 1 ? "male" : "female";
            var basedOn = GenerateBasedOn();
            var avatar = GenerateFace(gender);
            var name = GenerateName(gender);

            return new User()
            {
                Avatar = avatar,
                BasedOn = basedOn,
                Name = name
            };
        }

        private string GenerateBasedOn()
        {
            var possibleUsers = new[]
            {
                "xezno",
                "rip in peri peri",
                "Alex🌮",
                "Lazy Duchess",
                "houseofmous",
                "Hot",
                "dotequals",
                "The Architect"
            };
            return possibleUsers[random.Next(0, possibleUsers.Length)];
        }

        private string GenerateFace(string gender)
        {
            var response = Utils.GetHttpClient().GetAsync($"https://fakeface.rest/face/json?gender={gender}&minimum_age=20").Result;
            var responseString = response.Content.ReadAsStringAsync().Result;
            var deserializedResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);
            return deserializedResponse["image_url"];
        }

        private string GenerateName(string gender)
        {
            var firstNameFile = File.ReadAllLines($"Content/Names/{gender}.txt");
            var firstName = firstNameFile[random.Next(0, firstNameFile.Length)];
            
            var lastNameFile = File.ReadAllLines("Content/Names/last_names.txt");
            var lastName = lastNameFile[random.Next(0, lastNameFile.Length)];

            return $"{textInfo.ToTitleCase(firstName)} {textInfo.ToTitleCase(lastName)}";
        }

        private string GenerateMessage(User user)
        {
            var prefix = string.Join("", messageLog.TakeLast(ConfigBucket.context));

            Utils.Log(prefix);

            var values = new Dictionary<string, string>
            {
                { "temperature", ConfigBucket.temperature.ToString() },
                { "prefix", prefix },
                { "include_prefix", "false" },
                { "length", "50" },
                { "top_p", ConfigBucket.topP.ToString() },
                { "top_k", ConfigBucket.topK.ToString() }
            };

            var serializedValues = Newtonsoft.Json.JsonConvert.SerializeObject(values);

            var content = new StringContent(serializedValues, System.Text.Encoding.UTF8, "application/json");
            var response = Utils.GetHttpClient().PostAsync($"{ConfigBucket.apiEndpoint}/", content).Result;
            var responseString = response.Content.ReadAsStringAsync().Result;

            // Read as json
            var deserializedResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);
            var text = deserializedResponse["text"];

            // Replace \ns with new lines
            text.Replace("\\n", "\n");

            var splitMessages = text.Split('\n');
            foreach (var userMessage in splitMessages)
            {
                if (!userMessage.EndsWith(":") && !string.IsNullOrWhiteSpace(userMessage) && userMessage.Length > 1)
                {
                    messageLog.Add(
                        $"{user.BasedOn}:\n" +
                        $"{userMessage}\n" +
                        $"\n"
                    );
                    return userMessage;
                }
            }

            return "No message??";
        }
    }
}
