namespace GraduationProject.LogHandler.IService
{
    public interface ILoggerHandler
    {
        Task InsertLog(string? userId, string? tableName, string? recordId, object? oldData, object? newData, Type dataType);
        Task UpdateLog(string? userId, string? tableName, string? recordId, object? oldData, object? newData, Type dataType);
        Task DeleteLog(string? userId, string? tableName, string? recordId, object? oldData, object? newData, Type dataType);
        Task EventLog(string? userId, string? _event);
    }
}
