using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TestProject.Commands;

namespace TestProject.Masters
{
    public sealed class CommandMaster
    {
        #region Constructor
        private CommandMaster() 
        {
            _consoleMaster = ConsoleMaster.GetInstance();
        }
        #endregion

        #region Fields
        private static CommandMaster _instance;
        private static object _instanceLock = new object();

        private readonly ConsoleMaster _consoleMaster;
        #endregion

        #region Methods
        public static CommandMaster GetInstance()
        {
            lock (_instanceLock)
            {
                _instance ??= new CommandMaster();
                return _instance;
            }
        }

        public List<Type> GetCommandAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => typeof(ICommand).IsAssignableFrom(p)).ToList();
        }

        public void RunCommand(string command)
        {
            try
            {
                // Поиск классов, реализующих интерфейс ICommand
                List<Type> types = GetCommandAssemblies();

                Type findType = null;

                foreach (var type in types)
                {
                    // Поиск атрибута в найденных классах и сравнение его с введёной пользователем строкой
                    var findDescription = type.CustomAttributes.FirstOrDefault(p => p.AttributeType == typeof(CommandDescriptionAttribute)); 
                    if (findDescription != null && findDescription.ConstructorArguments.Count != 0 && command == (string)findDescription.ConstructorArguments[0].Value)
                    {
                        // Присваивание информации о типе 
                        findType = type;
                        break;
                    }
                }

                if (findType == null)
                {
                    // Не найден класс команды с нужным описанием
                    _consoleMaster.ShowErrorMessage("Команда не найдена! Воспользуйтесь командой /help, чтобы посмотреть список команд");
                }
                else
                {
                    // Создание экземпляра найденного класса и вызов его метода
                    ICommand instanceCommand = (ICommand)Activator.CreateInstance(findType);
                    instanceCommand.ExecuteCommand();
                }
            }
            catch (Exception ex)
            {
                _consoleMaster.ShowErrorMessage(ex.Message);
            }
        }
        #endregion
    }
}
