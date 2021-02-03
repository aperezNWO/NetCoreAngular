CREATE TABLE [dbo].[accessLogs] (
    [ID_column]  INT            IDENTITY (1, 1) NOT NULL,
    [PageName]   NVARCHAR (128) NOT NULL,
    [AccessDate] DATETIME       CONSTRAINT [DF__accessLog__Acces__24927208] DEFAULT (dateadd(hour,(1),getdate())) NOT NULL,
    [IpValue]    VARCHAR (32)   NULL,
    [LogType]    TINYINT        NULL,
    PRIMARY KEY CLUSTERED ([ID_column] ASC)
);

use [
select 
