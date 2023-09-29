namespace TestProject.Commands
{
    [CommandDescription("/yearsum", "Вывести сумму всех заключенных договоров за текущий год")]
    public class YearSumCommand : AbstractCommand, ICommand
    {
        public void ExecuteCommand()
        {
            var valueYearSum = DBMaster.YearSum();

            if (valueYearSum != 0)
                ConsoleMaster.ShowMessage($"Сумма всех заключенных договоров за текущий год: {valueYearSum}");
            else 
                ConsoleMaster.NoDbDataMessage();
        }
    }
}
