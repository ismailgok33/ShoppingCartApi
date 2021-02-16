USE CicekSepetiDB;
GO

create table Items
(
    -- Id int identity(1,1) primary key,
    Id UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
    Name nvarchar(max) not null,
    Price REAL not null,
    Stock int not null
)
go
