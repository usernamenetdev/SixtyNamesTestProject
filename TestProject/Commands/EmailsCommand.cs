using System.ComponentModel;

namespace TestProject.Commands
{
    [CommandDescription("/emails", "Вывести список e-mail уполномоченных лиц, заключивших договора за последние 30 дней, на сумму больше 40000")]
    public class EmailsCommand : AbstractCommand, ICommand
    {
        public void ExecuteCommand()
        {
            var valueEmails = DBMaster.Emails();

            if (valueEmails != null)
            {
                foreach (var result in valueEmails)
                    ConsoleMaster.ShowMessage($"{result.LastName} {result.FirstName} {result.MiddleName}: {result.Email}");
            }
            else
                ConsoleMaster.NoDbDataMessage();
        }
    }
}
