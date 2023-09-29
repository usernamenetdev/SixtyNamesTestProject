using TestProject.Masters;

namespace TestProject
{
    internal class Program 
    {
        #region Methods
        static void Main()
        {
            var consoleMaster = ConsoleMaster.GetInstance();
            var commandMaster = CommandMaster.GetInstance();

            consoleMaster.HelloMessage();

            var command = consoleMaster.ReadCommand();
            while (command != "/exit")
            {
                commandMaster.RunCommand(command);
                command = consoleMaster.ReadCommand();
            }  
        }
        #endregion
    }
}
