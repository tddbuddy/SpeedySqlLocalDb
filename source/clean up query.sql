
        declare @tmp table (cmd nvarchar(1024));
        insert into @tmp (cmd) select 'drop database ' + name from sys.databases where name like 'SpeedyDb_%' and is_cleanly_shutdown = 1;
        declare @cmd nvarchar(1024);
        while exists (select * from @tmp)
        begin
            select top 1 @cmd = cmd from @tmp;
            delete from @tmp where cmd = @cmd;
            exec(@cmd);
        end;


			select * from sys.databases where name like '%' and is_cleanly_shutdown = 1;

DECLARE 
@SQL NVARCHAR(1000)
, @DB_NAME NVARCHAR(100) = 'master'

SELECT *
FROM sys.master_files mf
WHERE mf.[type] = 0
AND mf.database_id = DB_ID(@DB_NAME)

PRINT @SQL
EXEC sys.sp_executesql @SQL