using System.ComponentModel;

namespace TestProject.Commands
{
    [CommandDescription ("/changestatus", "Изменить статус договора на \"Расторгнут\" для физических лиц, у которых есть действующий договор, и возраст которых старше 60 лет включительно")]
    public class ChangeStatusCommand : AbstractCommand, ICommand
    {
        public void ExecuteCommand()
        {
            var valueChangeStatus = DBMaster.ChangeStatus();

            if (valueChangeStatus.HasValue)
                ConsoleMaster.ShowMessage($"Расторгнуто договоров: {valueChangeStatus}");
            else
                ConsoleMaster.NoDbDataMessage();
        }
    }
}
