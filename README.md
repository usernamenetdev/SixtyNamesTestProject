# TestProject

Архитектурные решения:  
1. В данном приложении не используется ORM, запросы к БД и их обработчики разделены.  
2. Команды в приложении хранятся отдельно в каждом классе, имеют свой атрибут и реализуют интерфейс ICommand. (Pattern Strategy)  
3. Работа с БД, командами, консолью и Excel помещены в отдельные классы Masters, которые являются потокобезопасными синглтонами.  


Вы можете воспользоваться моей базой данных для проверки приложения, либо создать свою на основе следующих данных:
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
