using GraduationProject.Data.Entity;
using GraduationProject.LogHandler.IService;
using GraduationProject.LogHandler.Models;
using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Repository;
using GraduationProject.Shared.Attributes;

namespace GraduationProject.LogHandler.Service
{
    public class LoggerHandler : ILoggerHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;

        public LoggerHandler(UnitOfWork unitOfWork, IMailService mailService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mailService = mailService;
        }

        public async Task InsertLog(string? userId, string? tableName, string? recordId, object? oldData, object? newData, Type dataType)
        {
            var activityLogModel = new ActivityLogModel
            {
                UserId = userId,
                TableName = tableName,
                RecordId = recordId,
                Operation = LogOperation.Insert.Value,
                OldData = oldData,
                NewData = newData,
                DataType = dataType
            };

            await Log(activityLogModel);
        }

        public async Task UpdateLog(string? userId, string? tableName, string? recordId, object? oldData, object? newData, Type dataType)
        {
            var activityLogModel = new ActivityLogModel
            {
                UserId = userId,
                TableName = tableName,
                RecordId = recordId,
                Operation = LogOperation.Update.Value,
                OldData = oldData,
                NewData = newData,
                DataType = dataType
            };

            await Log(activityLogModel);
        }

        public async Task DeleteLog(string? userId, string? tableName, string? recordId, object? oldData, object? newData, Type dataType)
        {
            var activityLogModel = new ActivityLogModel
            {
                UserId = userId,
                TableName = tableName,
                RecordId = recordId,
                Operation = LogOperation.Delete.Value,
                OldData = oldData,
                NewData = newData,
                DataType = dataType
            };

            await Log(activityLogModel);
        }

        public async Task EventLog(string? userId, string? _event)
        {
            var activityLogModel = new ActivityLogModel
            {
                UserId = userId,
                Operation = LogOperation.Info.Value,
                Event = _event
            };

            await Log(activityLogModel);
        }

        private async Task Log(ActivityLogModel logModel)
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

        private ActivityLog? LogChanges(ActivityLogModel logModel)
        {
            var oldDataChanges = new List<string>();
            var newDataChanges = new List<string>();
            if (logModel.Operation != LogOperation.Info.Value)
            {
                // Get all properties of the entity using reflection
                var properties = logModel.DataType.GetProperties()
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
                    if (logModel.OldData != null && logModel.NewData != null && !Equals(originalValue, updatedValue))
                    {
                        oldDataChanges.Add($"{property.Name}: {originalValue}");
                        newDataChanges.Add($"{property.Name}: {updatedValue}");
                    }
                    else if (logModel.Operation == LogOperation.Insert.Value && logModel.NewData != null)
                    {
                        newDataChanges.Add($"{property.Name}: {updatedValue}");
                    }
                    else if (logModel.Operation == LogOperation.Delete.Value && logModel.OldData != null)
                    {
                        oldDataChanges.Add($"{property.Name}: {originalValue}");
                    }
                }
                if (!oldDataChanges.Any() && !newDataChanges.Any())
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
