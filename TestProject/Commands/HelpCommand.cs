using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TestProject.Masters;

namespace TestProject.Commands
{
    [CommandDescription("/help", "Отобразить список всех команд")]
    public class HelpCommand : AbstractCommand, ICommand
    {
        #region Fields

        private Dictionary<string, string> _helpList;

        #endregion

        #region Methods

        private Dictionary<string, string> FillHelpList()
        {
            Dictionary<string, string> commandList = new Dictionary<string, string>();
            // Поиск классов, реализующих интерфейс ICommand
            List<Type> types = CommandMaster.GetCommandAssemblies();

            foreach (var type in types)
            {
                // Поиск атрибута в найденных классах и заполнение коллекции описания команд
                var findDescription = type.CustomAttributes.FirstOrDefault(p => p.AttributeType == typeof(CommandDescriptionAttribute));
                if (findDescription != null && findDescription.ConstructorArguments.Count != 0 && (string)findDescription.ConstructorArguments[0].Value != "/exit" && (string)findDescription.ConstructorArguments[0].Value != "/help")
                {
                    commandList.Add((string)findDescription.ConstructorArguments[0].Value, (string)findDescription.ConstructorArguments[1].Value);
                }
            }
            commandList.Add("/exit", "Выйти из приложения");

            return commandList;
        }

        public void ExecuteCommand()
        {
            // Заполняем список команд при первом вызове /help (без возможности перезаполнить в рантайме)
            _helpList ??= FillHelpList();

            foreach(var command in _helpList)
            {
                ConsoleMaster.ShowMessage($"{command.Key} - {command.Value}");
            }
        }
        #endregion
    }
}
