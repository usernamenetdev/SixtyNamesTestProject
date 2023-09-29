# TestProject

<details>
	<summary>Текст задания</summary>
	<br>
Тестовое задание на позицию разработчик CRM  <br>
  <br>
Основные сущности:  <br>
1.	 Физическое лицо (имя, фамилия, отчество, пол, возраст, место работы, страна, город, адрес, e-mail, телефон, дата рождения)    <br>
2.	 Юридическое лицо (Наименование компании, ИНН, ОГРН, страна, город, адрес, e-mail, телефон)  <br>
3.	 Договор (Контрагент (юр. лицо), Уполномоченное лицо (физ. лицо), Сумма договора, Статус, Дата подписания)   <br>
<br>
	
Задание:  
Написать консольное приложение на языке c#, в котором необходимо реализовать классы для сущностей. Пользователь в приложении может выполнить следующие действия:  
1.	Вывести сумму всех заключенных договоров за текущий год.
2.	Вывести сумму заключенных договоров по каждому контрагенту из России.
3.	Вывести список e-mail уполномоченных лиц, заключивших договора за последние 30 дней, на сумму больше 40000.
4.	Изменить статус договора на "Расторгнут" для физических лиц, у которых есть действующий договор, и возраст которых старше 60 лет включительно.
5.	Создать отчет (текстовый файл, например, в формате xml, xlsx, json) содержащий ФИО, e-mail, моб. телефон, дату рождения физ. лиц, у которых есть действующие договора по компаниям, расположенных в городе Москва.  
  
Ограничение: Все данные по сущностям считываются/записываются из БД SQL (MS SQL, PostgreSQL, Oracle).  Данные в задачах 1-3 должны быть получены sql-запросом - без фильтрации на C#.В задаче 5 отчет формируется средствами c# на основе данных из БД.  

</details>

### Архитектурные решения:  
1. В данном приложении не используется ORM, запросы к БД и их обработчики разделены.  
2. Команды в приложении хранятся отдельно в каждом классе, имеют свой атрибут и реализуют интерфейс ICommand, что создает возможность легко добавлять и заменять команды даже в рантайме.
3. Код для работы с БД, командами, консолью и Excel помещен в отдельные классы Masters, которые являются потокобезопасными синглтонами.

### Пространства имён:  
1. Commands - пользовательские команды  
2. Masters - основные низкоуровневые классы для работы с приложением  
3. Models - модели данных  
	3.1. Reports - модели данных с вычисляемыми столбцами

### База данных
Вы можете воспользоваться уже созданной базой данных для проверки приложения, либо создать свою (MS SQL) на основе данных ниже. Строку подключения можно указать в `app.config`.
<details>
  <summary>Схема БД:</summary> 
  
  ![image](https://github.com/usernamenetdev/TestProject/assets/143216111/a06e2cdb-2820-46c4-a5b6-0d0ff808bed5)
    
</details>


<details>
  <summary>SQL запросы для создания таблиц</summary>  

  
  Company:  
```
 CREATE TABLE [dbo].[Company] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [COMPANYNAME] NVARCHAR (50)  NOT NULL,
    [ITN]         CHAR (12)      NOT NULL,
    [BIN]         CHAR (13)      NOT NULL,
    [COUNTRY]     NVARCHAR (50)  NOT NULL,
    [CITY]        NVARCHAR (50)  NOT NULL,
    [ADDRESS]     NVARCHAR (100) NOT NULL,
    [EMAIL]       NVARCHAR (50)  NOT NULL,
    [TEL]         VARCHAR (20)   NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
```
  Person:  
```
CREATE TABLE [dbo].[Person] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [LASTNAME]   NVARCHAR (50) NOT NULL,
    [FIRSTNAME]  NVARCHAR (50) NOT NULL,
    [MIDDLENAME] NVARCHAR (50) NOT NULL,
    [SEX]        NCHAR (1)     NOT NULL,
	[COUNTRY]    NVARCHAR (50) NOT NULL,
	[CITY]       NVARCHAR (50) NOT NULL,
	[ADDRESS]    NVARCHAR (100)NOT NULL,
	[EMAIL]		 NVARCHAR (50) NOT NULL,
	[TEL]        NVARCHAR (20) NOT NULL,
    [BIRTHDAY]   DATE          NOT NULL,
    [COMPANY_ID] INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([COMPANY_ID]) REFERENCES [dbo].[Company] ([Id])
);
```
  Contract:  
```
CREATE TABLE [dbo].[Contract] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [COMPANY_ID]      INT           NOT NULL,
    [PERSON_ID]       INT           NOT NULL,
    [CONTRACT_AMOUNT] MONEY         NOT NULL,
    [STATUS]          NVARCHAR (15) NOT NULL,
    [DATE]            DATE          NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([COMPANY_ID]) REFERENCES [dbo].[Company] ([Id])
);
```
</details>
