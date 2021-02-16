USE CicekSepetiDB;
GO

create table Items
(
    Id int identity
        constraint PK_Boards
            primary key,
    Name nvarchar(max) not null,
    Price REAL not null,
    Stock int not null
)
go
