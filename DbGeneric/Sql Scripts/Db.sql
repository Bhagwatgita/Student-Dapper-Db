create table Student(Id int primary key identity(1,1),FullName varchar(50),StudyLevel varchar(50))

insert into Student values('Ram Deshar','Bachelor')
insert into Student values('Ramesh Deshar','Master')
insert into Student values('Ramnath Deshar','PCL')
insert into Student values('Ramakant Deshar','CTEVT')

select * from Student