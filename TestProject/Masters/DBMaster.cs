using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using TestProject.Models;

namespace TestProject.Masters
{
    public sealed class DBMaster
    {
        #region Constructor
        private DBMaster() 
        {
            _connectionString = ConfigurationManager.ConnectionStrings["TestProject.Properties.Settings.TestDBConnectionString"].ConnectionString;
            _sqlConnection = new SqlConnection(_connectionString);

            _consoleMaster = ConsoleMaster.GetInstance();
        }
        #endregion

        #region Fields
        private static DBMaster _instance;
        private static object _instanceLock = new object();

        private readonly string _connectionString;
        private readonly SqlConnection _sqlConnection;
        private SqlDataReader _sqlDataReader;

        private ConsoleMaster _consoleMaster;
        #endregion

        #region Methods
        public static DBMaster GetInstance() 
        { 
            lock (_instanceLock)
            {
                _instance ??= new DBMaster();
            }
            return _instance; 
        }

        /// <summary>
        /// Запрашивает сумму договоров за текущий год
        /// </summary>
        /// <returns>Возвращает значение или null</returns>
        public decimal? YearSum()
        {
            try
            {
                _sqlConnection.Open();

                string command = "SELECT SUM(CONTRACT_AMOUNT) " 
                    + "FROM Contract " 
                    + "WHERE YEAR(DATE) = YEAR(GETDATE())"; // можно заменить на хранимые процедуры
                
                SqlCommand sqlCommand = new SqlCommand(command, _sqlConnection);

                decimal result = (decimal)sqlCommand.ExecuteScalar();
                
                return result;

            }
            catch (Exception ex)
            {
                _consoleMaster.ShowErrorMessage(ex.Message);
                return null;
            }
            finally { 
                _sqlConnection.Close(); 
            }
        }

        /// <summary>
        /// Запрашивает сумму контрактов для каждого контрагента из России
        /// </summary>
        /// <returns>Возвращает коллекцию или null</returns>
        public List<PartnerSumReport> PartnerSum()
        {
            try
            {
                _sqlConnection.Open();

                string command = "SELECT Company.COMPANYNAME, virtual.mysum " 
                    + "FROM Company " 
                    + "INNER JOIN "
                    + "(SELECT COMPANY_ID, SUM(CONTRACT_AMOUNT) AS mysum " 
                    + "FROM Contract " 
                    + "GROUP BY COMPANY_ID) AS virtual "
                    + "ON virtual.COMPANY_ID = Company.Id " 
                    + "WHERE Company.COUNTRY = N'Россия'";

                SqlCommand sqlCommand = new SqlCommand(command, _sqlConnection);

                _sqlDataReader = sqlCommand.ExecuteReader();

                var result = new List<PartnerSumReport>();

                while (_sqlDataReader.Read()) 
                {
                    result.Add(new PartnerSumReport()
                    {
                        CompanyName = _sqlDataReader.GetString(0),
                        Amount = _sqlDataReader.GetDecimal(1)
                    });
                }
                if (result.Count > 0)
                    return result;
                else return null;
            }
            catch (Exception ex)
            {
                _consoleMaster.ShowErrorMessage(ex.Message);
                return null;
            }
            finally
            {
                _sqlConnection.Close();
                _sqlDataReader?.Close();
            }
        }

        /// <summary>
        /// Запрашивает список e-mail уполномоченных лиц, заключивших договора за последние 30 дней, на сумму больше 40000
        /// </summary>
        /// <returns>Возвращает список или null</returns>
        public List<Person> Emails()
        {
            try
            {
                _sqlConnection.Open();

                string command = "SELECT LASTNAME, FIRSTNAME, MIDDLENAME, EMAIL " 
                    + "FROM Person " 
                    + "INNER JOIN Contract " 
                    + "ON Contract.PERSON_ID = Person.Id " 
                    + "WHERE Contract.DATE BETWEEN (DATEADD(DAY, -30, CONVERT(DATE, GETDATE()))) AND CONVERT(DATE, GETDATE())";

                SqlCommand sqlCommand = new SqlCommand(command, _sqlConnection);

                _sqlDataReader = sqlCommand.ExecuteReader();

                var result = new List<Person>();

                while (_sqlDataReader.Read())
                {
                    result.Add(new Person()
                    {
                        LastName = _sqlDataReader.GetString(0),
                        FirstName = _sqlDataReader.GetString(1),
                        MiddleName = _sqlDataReader.GetString(2),
                        Email = _sqlDataReader.GetString(3)
                    });
                }
                if (result.Count > 0)
                    return result;
                else return null;
            }
            catch (Exception ex)
            {
                _consoleMaster.ShowErrorMessage(ex.Message);
                return null;
            }
            finally { 
                _sqlConnection.Close();
                _sqlDataReader?.Close();
            }
        }

        /// <summary>
        /// Изменяет статус договоров на "Расторгнут" для лиц старше 60-ти
        /// </summary>
        /// <returns>Возвращает количество изменённых строк или null</returns>
        public int? ChangeStatus()
        {
            try
            {
                _sqlConnection.Open();

                string command = "UPDATE Contract " 
                    + "SET Contract.STATUS = N'Расторгнут' " 
                    + "FROM Contract " 
                    + "INNER JOIN Person " 
                    + "ON Contract.PERSON_ID = Person.Id " 
                    + "WHERE Person.AGE >= 60";

                SqlCommand sqlCommand = new SqlCommand(command, _sqlConnection);

                int result = sqlCommand.ExecuteNonQuery();

                return result;
            }
            catch (Exception ex)
            {
                _consoleMaster.ShowErrorMessage(ex.Message);
                return null;
            }
            finally {
                _sqlConnection.Close();
            }
        }

        /// <summary>
        /// Запрашивает ФИО, e-mail, моб. телефон, дату рождения физ. лиц, у которых есть действующие договора по компаниям, расположенных в городе Москва
        /// </summary>
        /// <returns>Возвращает DataTable</returns>
        public System.Data.DataTable GetCurrentContracts()
        {
            try
            {
                _sqlConnection.Open();

                string command = "SELECT LASTNAME + N' ' + FIRSTNAME + N' ' + MIDDLENAME AS FULLNAME, Person.EMAIL, Person.TEL, BIRTHDAY " 
                    + "FROM Person " 
                    + "INNER JOIN Company " 
                    + "ON Company.Id = Person.COMPANY_ID " 
                    + "INNER JOIN Contract " 
                    + "ON Contract.PERSON_ID = Person.Id " 
                    + "WHERE Company.CITY = N'Москва' AND Contract.STATUS = N'Действующий'"; 

                SqlCommand sqlCommand = new SqlCommand(command, _sqlConnection);

                System.Data.DataTable dataTable = new System.Data.DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(sqlCommand))
                    da.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                _consoleMaster.ShowErrorMessage(ex.Message);
                return null;
            }
            finally 
            { 
                _sqlConnection.Close(); 
            }
        }
        #endregion
    }
}
