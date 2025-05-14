 // Discord.Net-Snippets, Copyright(C) 2025  by Vivian Bellfore

 //   This file is free software: you can redistribute it and/or modify
 //   it under the terms of the GNU General Public License as published by
 //   the Free Software Foundation, either version 3 of the License, or
 //   (at your option) any later version.

 //   This file is distributed in the hope that it will be useful,
 //   but WITHOUT ANY WARRANTY; without even the implied warranty of
 //   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 //   GNU General Public License for more details.

 //   You should have received a copy of the GNU General Public License
 //   along with this file.  If not, see <https://www.gnu.org/licenses/>.


using Discord;
using Discord.WebSocket;

using System;
using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;



namespace YOURBOTPROJECTNAMESPACE
{
    /// <summary>
    /// This class is handeling all slash command related functions.
    /// </summary>
    public class CommandManager
    {
        /// <summary>
        /// This function is register slash commands in our system and adding them to: <seealso cref="SlashCommand.slashCommandList"/><para/>
        /// This function needs to be initialized after your bot is started and ready.
        /// </summary>
        public static void SetupCommands()
        {
            // You need to build construction classes for your commands. The example is included in the GitHub repo.
            // This is an example how to add a new command:
            new AdminCommands("admin", "Using server admin functions.");
        }

        /// <summary>
        /// This list contains strings with command names that are using modals.<br/>
        /// We can not respond on a command that will use a modal, so we have to check this in <seealso cref="SlashCommandExecutedHandler"/> first. The modal will do the respond.
        /// </summary>
        public static List<string> commandsWithModal = new List<string>();

        /// <summary>
        /// This handler function needs to be used by the event "SlashCommandExecuted" from the Discord.Net API.
        /// This fnction is handeling the SlashCommandExecuted from all slash commands incoming from the API.<para/>
        /// Functions:<br/>
        /// - Sending respond.<br/>
        /// - Execute <seealso cref="SlashCommand.OnCommandExecute(SocketSlashCommand)"/> function if all checks are valid.
        /// </summary>
        public static async Task SlashCommandExecutedHandler(SocketSlashCommand command)
        {
            if (!commandsWithModal.Contains(command.Data.Options.First().Name))
                await command.RespondAsync("Command is loading, please wait...", ephemeral: true);

            if (await IsCommandAllowedToUse(command) == false) return;

            SlashCommand cmd = SlashCommand.slashCommandList.First(c => c.Name == command.Data.Name);
            if (cmd == null) await CommandInvalid(command);

            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += async (sender, e) =>
            {
                try
                {
                    await cmd.OnCommandExecute(command);
                }
                catch (Exception exceptionMessage)
                {
                    string messageText = "Error: Something went wrong.";

                    if (command.HasResponded)
                        await command.ModifyOriginalResponseAsync(func => func.Content = messageText);
                    else
                        await command.RespondAsync(messageText, ephemeral: true);

                    Console.WriteLine($"# CommandManager, SlashCommandExecuteHandler\n{exceptionMessage}");
                }
            };
            backgroundWorker.RunWorkerAsync();

            await Task.Delay(1);
        }

        /// <summary>
        /// This function is checking conditions for a user to decide if he is allowed to use a command.<para/>
        /// Conditions checked:<br/>
        /// - IsBot<br/>
        /// - IsWebhook<br/>
        /// - IsDMInteraction<br/>
        /// - IsUserTimedOut<br/>
        /// </summary>
        private static async Task<bool> IsCommandAllowedToUse(SocketSlashCommand command)
        {
            if (command.User.IsBot || command.User.IsWebhook)
                return false; // we dont need to send a bot a message ;)

            if (command.IsDMInteraction)
            {
                string message = "Commands are not allowed to use in DM´s.";
                await command.ModifyOriginalResponseAsync(func => { func.Content = message; });
                return false;
            }

            IGuild guild = Program._client.GetGuild((ulong)command.GuildId);
            if (guild != null)
            {
                IGuildUser guildUser = await guild.GetUserAsync(command.User.Id);

                if (guildUser != null && guildUser.TimedOutUntil.HasValue && guildUser.TimedOutUntil > DateTime.Now)
                {
                    string message = "This action is not allowed while a user has a timeout.";
                    await command.ModifyOriginalResponseAsync(func => { func.Content = message; });
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// This function is triggered if no valid command name was given. This function is sending a response.
        /// </summary>
        public static async Task CommandInvalid(SocketSlashCommand command)
        {
            string message = "Command is outdated and needs to be removed or updated.";

            if (command.HasResponded)
                await command.ModifyOriginalResponseAsync(func => { func.Content = message; });
            else
                await command.RespondAsync(message, ephemeral: true);

            Console.WriteLine($"# CommandManager, CommandInvalid\nCommand name was outdated! No such command is registered.\n" +
                $"Name was: /{command.Data.Name} {command.Data.Options.First().Name}");
        }
    }



    /// <summary>
    /// This class is register and handling slash commands for our internal system.
    /// </summary>
    public class SlashCommand : SlashCommandBuilder
    {
        /// <summary>
        /// This list contains all registered commands.
        /// </summary>
        public static List<SlashCommand> slashCommandList = new List<SlashCommand>();

        /// <summary>
        /// Constructor for the SlashCommand.
        /// </summary>
        public SlashCommand(string name, string description)
        {
            WithName(name);
            WithDescription(description);
            slashCommandList.Add(this);
        }

        /// <summary>
        /// Executing the command.
        /// </summary>
        public virtual async Task OnCommandExecute(SocketSlashCommand command)
        {
            await Task.FromResult(0);
        }
    }



    /// <summary>
    /// This class is the constructor for command objects that are setting help texts for commands.
    /// </summary>
    public class CommandObject
    {
        /// <summary>
        /// This list contains all registered command objects.
        /// </summary>
        public static List<CommandObject> commandObjectList = new List<CommandObject>();

        public string GroupName { get; set; }
        public string Name { get; set; }
        public string DescriptionGerman { get; set; }
        public string DescriptionEnglish { get; set; }


        public CommandObject(string groupName, string name, string descriptionGerman, string descriptionEnglish)
        {
            GroupName = groupName;
            Name = name;
            DescriptionGerman = descriptionGerman;
            DescriptionEnglish = descriptionEnglish;
            commandObjectList.Add(this);
        }
        

        public virtual async Task CommandFunction(SocketSlashCommand command)
        {
            await Task.FromResult(0);
        }
    }
}
