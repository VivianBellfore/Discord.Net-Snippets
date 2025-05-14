# Discord.Net-Snippets
This are some snippets from my Discord.Net bot projects that could be helpful for others to build their own bot for Discord in CSharp.

<br>

# How to add slash commands to your bot?
If you want to use slash commands with your bot, you need to have some system that will manage them.
My "CommandManager" is a framework for slash commands with sub commands. This commands are build like this: "/admin help"
They have a command name "admin" and a sub command name "help". Discord will only allow you to add 50 commands, that is why i added sub commands.
With this system you can add up to 50 commands like "/admin" and every command can have many sub commands.

The command "/admin" can contain sub commands like:
- help
- kick
- ban
- timeout

To use them, you would write "/admin help" or "/admin kick" ect.

If you are using my "CommandManager" you will also need "AdminCommands" as an example how to build a slash command and "GetAdminHelp" as an example how to build a sub command.

Installation
1. Add "CommandManager", "AdminCommands" and "GetAdminHelp" to your bot as new classes.
2. Change the Namespace in all three classes to your projects name space.
3. Add the Discord.Net API event handler "SlashCommandExecuted" and trigger the "CommandManager.SlashCommandExecutedHandler" function there. Would look like this:
   ```CS
   internal static DiscordSocketClient.SlashCommandExecuted += CommandManager.SlashCommandExecutedHandler;
   ```
4. Initialize "CommandManager.SetupCommands()" on bot start after your bot is ready.
5. Add the new command. This is not included in the command manager, but here is a function u can use to update a command:
   ```CS
    /// <summary>
    /// This function is triggered by console input and will update a command for commandBuilder from discord.
    /// </summary>
    public async Task UpdateCommand(string commandName)
    {
        IReadOnlyCollection<SocketApplicationCommand> commands = await Program._client.GetGlobalApplicationCommandsAsync();
    
        if (commands.Where(cmd => cmd.Name == commandName).Any() == false)
        {
            Console.WriteLine($"[ UpdateCommand ] Command name {commandName} is invalid!");
            return;
        }
    
        if (SlashCommand.slashCommandList.Where(cmd => cmd.Name == commandName).Any() == false)
        {
            Console.WriteLine($"[ UpdateCommand ] Command name {commandName} is not found in slashCommandList list!");
            return;
        }
    
        SocketApplicationCommand thisCommand = commands.Where(cmd => cmd.Name == commandName).First();
    
        await thisCommand.DeleteAsync();
    
        SlashCommand myCommand = SlashCommand.slashCommandList.Where(cmd => cmd.Name == commandName).First();
    
        await Program._client.Rest.CreateGlobalCommand(myCommand.Build());
    
        Console.WriteLine($"Command {commandName} was updated.");
    }
   ```
   This function awaits the command name, in this example it would be "admin". You only need to update the command, not the sub commands!
   I decided to make the update process for command manually to avoid unnecessary responds to the API. This could be made automatically on bot start but this is up to your liking.
