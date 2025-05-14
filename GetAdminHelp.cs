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

using System.Threading.Tasks;



namespace YOURBOTPROJECTNAMESPACE
{
    /// <summary>
    /// This class is building and managing the <paramref name="admin help"/> command.<para/>
    /// </summary>
    public class GetAdminHelp : CommandObject
    {
        /// <summary>
        /// This is the strukt for the help command informations in two different languages.
        /// </summary>
        public GetAdminHelp() : base("admin", "help", "Liste mit allen Befehlen für Administratoren.", "List of all commands for admins.") { }

        /// <summary>
        /// This function is handling command conditions and executing other functions.<para/>
        /// Function executed by:
        /// <seealso cref="CommandManager.SlashCommandExecutedHandler(SocketSlashCommand)"/>
        /// </summary>
        public async override Task CommandFunction(SocketSlashCommand command)
        {
            // if you have any kind of language selection, make your check here and change the behavior in line 54 and 57.
            // I will currently not upload my language system, but i left the language command helper, so you can use it.
            
            // If this is set to 1 it is in english. If this is set to 0 it is in german.
            int language = 1;

            string message = "";

            foreach (CommandObject obj in commandObjectList)
            {
                if (obj.GroupName == "admin")
                    message += language == 0 ? $"\n`/admin {obj.Name}` - {obj.DescriptionGerman}" : $"\n`/admin {obj.Name}` - {obj.DescriptionEnglish}";
            }

            string title = language == 0 ? "Administratorbefehle" : "Adminsitration commands";

            var embedBuiler = new EmbedBuilder()
                .WithTitle(title)
                .WithDescription(message)
                .WithColor(Color.Orange);

            await command.ModifyOriginalResponseAsync(func => { func.Content = ""; func.Embed = embedBuiler.Build(); });
        }
    }
}
