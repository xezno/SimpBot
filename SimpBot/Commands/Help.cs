﻿using Disco;
using Discord;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace SimpBot.Commands
{
    public class Help : Command
    {
        public override string[] Aliases => new string[] { "help", "h", "?" };
        public override string[] Syntax => new string[] { };
        public override string Description => "Shows all available commands.";
        public override string Icon => "💁";
        public override int MinArgs => 0;
        public override int MaxArgs => 0;

        public override void Run(CommandArgs args)
        {
            EmbedBuilder eb = Disco.Utils.BuildDefaultEmbed();
            eb.WithTitle($"{Icon} Commands");

            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (t.Namespace == null)
                    continue;

                if (t.Namespace.Equals("SimpBot.Commands", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!t.IsAbstract && t.BaseType == typeof(Command))
                    {
                        var cmd = (Command)Activator.CreateInstance(t);

                        var cmdSyntaxStr = "";
                        foreach (string s in cmd.Syntax)
                        {
                            cmdSyntaxStr += $" [{s}]";
                        }

                        var cmdAliasesStr = "";
                        for (int i = 0; i < cmd.Aliases.Length; ++i)
                        {
                            if (i > 0 && i < cmd.Aliases.Length)
                                cmdAliasesStr += " / ";
                            cmdAliasesStr += cmd.Aliases[i];
                        }
                        eb.AddField("`" + ConfigBucket.prefix + cmdAliasesStr + cmdSyntaxStr + "`", cmd.Description);
                    }
                }
            }

            args.Message.Channel.SendMessageAsync("", false, eb.Build());
        }
    }
}
