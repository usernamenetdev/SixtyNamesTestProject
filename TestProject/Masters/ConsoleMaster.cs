using System;

namespace TestProject.Masters
{
    public sealed class ConsoleMaster
    {
        #region Constructor
        private ConsoleMaster() { }
        #endregion

        #region Fields
        private static ConsoleMaster _instance;
        private static object _instanceLock = new object();
        #endregion

        #region Methods
        public static ConsoleMaster GetInstance()
        {
            lock (_instanceLock) 
            { 
                _instance ??= new ConsoleMaster();
                return _instance;
            }
        }

        /// <summary>
        /// Окрашивает строку в красный, используется для обозначения ошибок
        /// </summary>
        /// <param name="message">Текст ошибки</param>
        public void ShowErrorMessage(string message = null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (message == null)
                Console.WriteLine("Произошла программная ошибка");
            else
                Console.WriteLine(message);
            Console.ResetColor();
        }

        public void HelloMessage()
        {
            Console.WriteLine("Воспользуйтесь командой /help, чтобы посмотреть список команд");
        }

        public void NoDbDataMessage() => Console.WriteLine("Данные не найдены в БД");

        public void ShowMessage(string message) => Console.WriteLine(message);

        public string ReadCommand()
        {
            Console.Write("\n> ");
            return Console.ReadLine().ToLower();
        }
        #endregion
    }
}
