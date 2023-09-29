using System.ComponentModel;

namespace TestProject.Commands
{
    [CommandDescription("/partnersum", "Вывести сумму заключенных договоров по каждому контрагенту из России")]
    public class PartnerSumCommand : AbstractCommand, ICommand
    {
        public void ExecuteCommand()
        {
            var valuePartnerSum = DBMaster.PartnerSum();

            if (valuePartnerSum != null)
            {
                foreach (var result in valuePartnerSum)
                    ConsoleMaster.ShowMessage($"Сумма контрактов компании {result.CompanyName}: {result.Amount}");
            }
            else
                ConsoleMaster.NoDbDataMessage();
        }
    }
}
