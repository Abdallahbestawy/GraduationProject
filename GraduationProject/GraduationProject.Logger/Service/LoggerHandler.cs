using GraduationProject.Data.Entity;
using GraduationProject.LogHandler.Models;
using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Repository;
using GraduationProject.Shared.Attributes;
using Microsoft.IdentityModel.Tokens;

namespace GraduationProject.LogHandler.Service
{
    public class LoggerHandler<T>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;

        public string? UserId { get; set; }

        public string? TableName { get; set; }

        public string? RecordId { get; set; }

        public T? OldData { get; set; }

        public T? NewData { get; set; }

        public LoggerHandler(UnitOfWork unitOfWork, IMailService mailService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mailService = mailService;
        }

        public async Task InsertLog(string? userId,string? tableName,string? recordId,T? oldData,T? newData)
        {
            var activityLogModel = new ActivityLogModel<T>
            {
                UserId = userId,
                TableName = tableName,
                RecordId = recordId,
                Operation = LogOperation.Insert.Value,
                OldData = oldData,
                NewData = newData
            };

            await Log(activityLogModel);
        }

        public async Task UpdateLog(string? userId, string? tableName, string? recordId, T? oldData, T? newData)
        {
            var activityLogModel = new ActivityLogModel<T>
            {
                UserId = userId,
                TableName = tableName,
                RecordId = recordId,
                Operation = LogOperation.Update.Value,
                OldData = oldData,
                NewData = newData
            };

            await Log(activityLogModel);
        }

        public async Task DeleteLog(string? userId, string? tableName, string? recordId, T? oldData, T? newData)
        {
            var activityLogModel = new ActivityLogModel<T>
            {
                UserId = userId,
                TableName = tableName,
                RecordId = recordId,
                Operation = LogOperation.Delete.Value,
                OldData = oldData,
                NewData = newData
            };

            await Log(activityLogModel);
        }

        public async Task EventtLog(string? userId, string? _event)
        {
            var activityLogModel = new ActivityLogModel<T>
            {
                UserId = userId,
                Operation = LogOperation.Info.Value,
                Event = _event
            };

            await Log(activityLogModel);
        }

        private async Task Log(ActivityLogModel<T> logModel)
        {
            try
            {
                var newLog = LogChanges(logModel);
                if (newLog == null)
                    return;
                await _unitOfWork.ActivityLogs.AddAsync(newLog);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "LoggerHandler",
                    MethodName = "Log",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
            }
        }

        private ActivityLog? LogChanges(ActivityLogModel<T> logModel)
        {
            var oldDataChanges = new List<string>();
            var newDataChanges = new List<string>();
            if (logModel.Operation != LogOperation.Info.Value)
            {
                // Get all properties of the entity using reflection
                var properties = typeof(T).GetProperties()
                    .Where(prop => prop.CanRead && prop.CanWrite && !prop.IsDefined(typeof(IgnoreLoggingAttribute), true))
                    .ToList();

                foreach (var property in properties)
                {
                    object? originalValue = null;
                    object? updatedValue = null;

                    if (logModel.OldData != null)
                        originalValue = property.GetValue(logModel.OldData);

                    if (logModel.NewData != null)
                        updatedValue = property.GetValue(logModel.NewData);

                    // Compare the values
                    if (!(logModel.OldData == null || logModel.NewData == null))
                    {
                        if (!object.Equals(originalValue, updatedValue))
                        {
                            oldDataChanges.Add($"{property.Name}: {originalValue}");
                            newDataChanges.Add($"{property.Name}: {updatedValue}");
                        }
                    }
                    else if (logModel.Operation == LogOperation.Insert.Value)
                        newDataChanges.Add($"{property.Name}: {updatedValue}");
                    else if (logModel.Operation == LogOperation.Delete.Value)
                        oldDataChanges.Add($"{property.Name}: {originalValue}");
                }
                if (oldDataChanges.IsNullOrEmpty() && newDataChanges.IsNullOrEmpty())
                    return null;
            }

            var activityLog = new ActivityLog
            {
                UserId = logModel.UserId,
                TableName = logModel.TableName,
                RecordId = logModel.RecordId,
                Operation = logModel.Operation,
                OldData = string.Join(", ", oldDataChanges),
                NewData = string.Join(", ", newDataChanges),
                LogTime = DateTime.UtcNow
            };

            return activityLog;
        }
    }
}
