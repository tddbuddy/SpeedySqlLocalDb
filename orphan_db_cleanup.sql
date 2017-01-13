declare @tmp table (cmd nvarchar(1024));
insert into @tmp (cmd) select 'drop database ' + name from sys.databases where name like 'SpeedyDb_%' and is_cleanly_shutdown = 1;
declare @cmd nvarchar(1024);
while exists (select * from @tmp)
begin
    select top 1 @cmd = cmd from @tmp;
    delete from @tmp where cmd = @cmd;
	begin try
		exec(@cmd);
	end try
	begin catch
		select 1;
	end catch
end