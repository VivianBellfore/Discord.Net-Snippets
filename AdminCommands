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
using System.Threading.Tasks;



namespace YOURBOTPROJECTNAMESPACE
{
    /// <summary>
    /// This class is building and executing the global <paramref name="admin"/> command.<para/>
    /// </summary>
    public class AdminCommands : SlashCommand
    {
        /// <summary>
        /// This constructor is a builder for a global command called <paramref name="admin"/>.<br/>
        /// Constructor executed by:
        /// <seealso cref="CommandManager.SetupCommands"/><para/>
        /// </summary>
        public AdminCommands(string name, string description) : base(name, description)
        {
            // The "AdminCommands" class is handeling the command "/admin" and will trigger the sub command functions.
            // You need to build the sub commands in their own classes each. An example is included in the GitHub repo named "GetAdminHelp" for the sub command "help".
            new GetAdminHelp();
            AddOption(new SlashCommandOptionBuilder().WithName("help").WithDescription("All admin commands explained.").WithType(ApplicationCommandOptionType.SubCommand));
        }



        /// <summary>
        /// This function is handling command conditions and executing other functions.<para/>
        /// Function executed by:
        /// <seealso cref="CommandManager.SlashCommandExecutedHandler(SocketSlashCommand)"/>
        /// </summary>
        public async override Task OnCommandExecute(SocketSlashCommand command)
        {
            try
            {
                CommandObject cmd = CommandObject.commandObjectList.First(x => x.GroupName == command.Data.Name && x.Name == command.Data.Options.First().Name);
                await cmd.CommandFunction(command);
            }
            catch (Exception exceptionMessage)
            {
                Console.WriteLine($"# AdminCommands, OnCommandExecute\n Command was not fetched from commandObjectList.\n{exceptionMessage}");
                await CommandManager.CommandInvalid(command);
            }
        }
    }
}
