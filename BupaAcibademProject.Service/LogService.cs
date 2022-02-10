using BupaAcibademProject.DataAccessLayer;
using BupaAcibademProject.Domain.Entities;
using BupaAcibademProject.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Service
{
    public class LogService : ILogService
    {
        //private readonly IRepository<ErrorLog> _repositoryErrorLog;
        //private readonly IRepository<EntityLog> _repositoryEntityLog;
        //private readonly IRepository<ServiceHistory> _repositoryServiceHistory;
        private readonly IUserAccessor _userAccessor;

        private List<EntityLog> _EntityLogs = new List<EntityLog>();

        public LogService(/*LogDbContext dbContext, */IUserAccessor userAccessor)
        {
            //_repositoryErrorLog = new Repository<ErrorLog>(dbContext);
            //_repositoryEntityLog = new Repository<EntityLog>(dbContext);
            //_repositoryServiceHistory = new Repository<ServiceHistory>(dbContext);
            _userAccessor = userAccessor;
        }

        public async Task ClearEntityHistories()
        {
            try
            {
                _EntityLogs.Clear();
            }
            catch (Exception ex)
            {
                await LogException(ex);
            }
        }

        public async Task LogDeleteHistory<T>(int id, bool isTransactional) where T : Entity
        {
            try
            {
                var log = new EntityLog()
                {
                    TableId = id,
                    TableName = typeof(T).Name,
                    InsurerId = _userAccessor.User?.Id,
                    Changes = null,
                    LogType = Domain.Enums.LogType.DELETE,
                    ClientIP = _userAccessor.ClientIP
                };
                if (isTransactional)
                {
                    _EntityLogs.Add(log);
                }
                else
                {
                    //await _repositoryEntityLog.Add(log);
                }

            }
            catch (Exception ex)
            {
                await LogException(ex);
            }
        }

        public async Task LogEntityHistory<T>(T data, T old, bool isTransactional) where T : Entity
        {
            try
            {
                var log = GetEntityLog(data, old);
                if (log != null)
                {
                    if (isTransactional)
                    {
                        _EntityLogs.Add(log);
                    }
                    else
                    {
                        //await _repositoryEntityLog.Add(log);
                    }
                }
            }
            catch (Exception ex)
            {
                await LogException(ex);
            }
        }

        public async Task LogEntityHistory(EntityLog log)
        {
            try
            {
                //await _repositoryEntityLog.Add(log);
            }
            catch (Exception ex)
            {
                await LogException(ex);
            }
        }

        public async Task<string> LogException(Exception ex)
        {
            try
            {
                if (!(ex is BusException))
                {
                    var log = new ErrorLog()
                    {
                        ClientIP = _userAccessor.ClientIP,
                        ErrorMessage = GetErrorMessage(ex, 0),
                        RequestLink = _userAccessor.RequestLink,
                        UserId = _userAccessor.User != null ? (int?)_userAccessor.User.Id : null
                    };

                    //await _repositoryErrorLog.Add(log);
                }
            }
            catch { }

            return GetErrorString(ex);
        }

        public async Task SaveServiceHistory(ServiceHistory history)
        {
            try
            {
                if (history == null)
                {
                    return;
                }
                if (history.Id > 0)
                {
                    //await _repositoryServiceHistory.Update(history);
                }
                else
                {
                    //await _repositoryServiceHistory.Add(history);
                }
            }
            catch (Exception ex)
            {
                await LogException(ex);
            }
        }

        public async Task WriteEntityHistories()
        {
            try
            {
                foreach (var item in _EntityLogs)
                {
                    //await _repositoryEntityLog.Add(item);
                }
            }
            catch (Exception ex)
            {
                await LogException(ex);
            }
        }

        private string GetErrorString(Exception ex)
        {
            var message = ex.Message;
            if (ex.InnerException != null)
            {
                message += Environment.NewLine + GetErrorString(ex.InnerException);
            }
            return message;
        }

        private string GetErrorMessage(Exception ex, int level)
        {
            var message = "";
            if (level > 0)
            {
                message += string.Concat(Enumerable.Repeat(Environment.NewLine, 2));
                message += string.Concat(Enumerable.Repeat("-", 80)) + Environment.NewLine;
            }
            message += ex.Message + Environment.NewLine;
            message += ex.StackTrace + Environment.NewLine;

            if (ex.InnerException != null)
            {
                message += GetErrorMessage(ex.InnerException, level + 1);
            }
            return message;
        }

        private EntityLog GetEntityLog<T>(T data, T old) where T : Entity
        {
            var log = new EntityLog()
            {
                TableId = data.Id,
                TableName = typeof(T).Name,
                InsurerId = _userAccessor.User?.Id,
                Changes = null,
                LogType = data.Deleted ? Domain.Enums.LogType.DELETE : Domain.Enums.LogType.INSERT,
                ClientIP = _userAccessor.ClientIP
            };
            if (old != null)
            {
                log.LogType = Domain.Enums.LogType.UPDATE;
                log.Changes = EntityMetaData.GetChanges(data, old);
                if (string.IsNullOrEmpty(log.Changes))
                {
                    return default;
                }
            }
            return log;
        }
    }

    public class EntityMetaData
    {
        private static List<ClassField> _TypeFields = new List<ClassField>();
        private static object _Locker = new object();
        private static string[] _IgnoredFields = new string[] { "CreateDate", "UpdateDate", "Deleted" };

        public static string GetChanges<T>(T data, T old)
        {
            var fields = GetFields<T>();
            var jData = new Newtonsoft.Json.Linq.JObject();
            var changed = false;

            foreach (var item in fields)
            {
                var value = item.Value.Invoke(data);
                var oldValue = item.Value.Invoke(old);
                if (!Equals(value, oldValue))
                {
                    changed = true;
                    jData[item.Key] = new Newtonsoft.Json.Linq.JArray();
                    ((Newtonsoft.Json.Linq.JArray)jData[item.Key]).Add(oldValue);
                    ((Newtonsoft.Json.Linq.JArray)jData[item.Key]).Add(value);
                }
            }
            if (!changed)
            {
                return default;
            }
            return jData.ToString();
        }

        private static Dictionary<string, Func<object, object>> GetFields<T>()
        {
            lock (_Locker)
            {
                var type = _TypeFields.FirstOrDefault(a => a.ClassType == typeof(T));
                if (type == null)
                {
                    type = new ClassField(typeof(T));
                    foreach (var item in type.ClassType.GetProperties())
                    {
                        if (item.PropertyType.IsValueType && !_IgnoredFields.Contains(item.Name))
                        {
                            type.Fields[item.Name] = PropGetter(item);
                        }
                    }
                }
                return type.Fields;
            }
        }

        private static Func<object, object> PropGetter(PropertyInfo propertyInfo)
        {
            var data = Expression.Parameter(typeof(object), "data");
            var dataConverted = Expression.Convert(data, propertyInfo.DeclaringType);
            if (propertyInfo.GetMethod != null)
            {
                var body = Expression.Call(dataConverted, propertyInfo.GetGetMethod());
                return Expression.Lambda<Func<object, object>>(Expression.Convert(body, typeof(object)), data).Compile();
            }
            return null;
        }

        internal class ClassField
        {
            public Type ClassType { get; set; }
            public Dictionary<string, Func<object, object>> Fields { get; set; }

            public ClassField(Type type)
            {
                ClassType = type;
                Fields = new Dictionary<string, Func<object, object>>();
            }
        }

    }
}
