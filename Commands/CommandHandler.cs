

using System.Security.Cryptography.X509Certificates;

namespace Luckyscanner.Commands
{
    public class CommandHandler
    {
        List<Command> commands;
        public CommandHandler()
        {
            this.commands = new List<Command>();

            commands.Add(new Scan("lucky"));
        }

        public string runCommand(string cmd)
        {
            string[] sp = cmd.Split('-');
            String commandName = cmd.Split().First(); //Takes the first element from command
            cmd = cmd.Replace(commandName + " ", "");
            string[] parameters = cmd.Split();
            var dic = new Dictionary<string, string>();
            for (int i = 0; i < parameters.Length; i += 2)
            {
                dic.Add(new String(parameters[i].Skip(1).ToArray()), parameters[i + 1]);
            }

            foreach (Command c in commands)
            {
                if (c.name.ToLower() == commandName.ToLower())
                {
                    return c.execute(dic);
                }
            }
            return $"Command {cmd} does not exist!";
        }
    }
}
