using TestProject.Masters;

namespace TestProject.Commands
{
    public abstract class AbstractCommand
    {
        #region Constructor
        public AbstractCommand()
        {
            CommandMaster = CommandMaster.GetInstance();
            ConsoleMaster = ConsoleMaster.GetInstance();
            DBMaster = DBMaster.GetInstance();
        }
        #endregion

        #region Properties
        public CommandMaster CommandMaster { get; private set; }
        public ConsoleMaster ConsoleMaster { get; private set; }
        public DBMaster DBMaster { get; private set; }
        #endregion
    }
}
