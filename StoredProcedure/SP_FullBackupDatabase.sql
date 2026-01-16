 create Procedure SP_FullBackupDatabase
(
@BackupLocation varchar(260),
@SourceFilePath varchar(255) output
)
as

Begin
	Set NoCount On;

	declare @DatabaseName SysName = DB_Name();

	select @DatabaseName

	Declare 
		@BackupFile Nvarchar(260),
		@Sql Nvarchar(Max),
		@DateTime varchar(30)


		set @DateTime = Convert(Varchar(8), GetDate(), 112) + '_' + 
		Replace(Convert(Varchar(8), GetDate(), 108), ':', '')


		set @BackupFile =@BackupLocation + 'Backup_' + @DateTime + '.bak'

		set @SourceFilePath = @BackupFile

		set @Sql = 'Backup Database [' + @DatabaseName +'] To disk = ''' + @BackupFile
			+ ''' with init , name = ''Full Backup''';

		exec(@Sql);

End
