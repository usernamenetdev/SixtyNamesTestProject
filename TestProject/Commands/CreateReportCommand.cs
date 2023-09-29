using System.ComponentModel;
using TestProject.Masters;

namespace TestProject.Commands
{
    [CommandDescription("/createreport", "Создать отчет по физ. лицам, у которых есть действующие договора по компаниям, расположенных в городе Москва")]
    public class CreateReportCommand : AbstractCommand, ICommand
    {
        public void ExecuteCommand()
        {
            var xlsxMaster = XlsxMaster.GetInstance();
            var dataTable = DBMaster.GetCurrentContracts();

            string excelFilePath = xlsxMaster.CreateReport(dataTable);

            if (excelFilePath != null)
                ConsoleMaster.ShowMessage($"Отчёт сохранён в {excelFilePath}");
            else
                ConsoleMaster.ShowErrorMessage("Отчёт не был сохранён");
        }
    }
}
