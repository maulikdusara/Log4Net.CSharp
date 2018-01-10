using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Log4NetExtension
{
    public static class Log4NetExtensions
    {
        #region Internal Members

        private enum enLogLevel
        {
            Log,
            Debug,
            Error,
            Fatal,
            Info,
            Warn
        }

        static readonly log4net.Core.Level logLevel = new log4net.Core.Level(Configurations.customLogLevel, Configurations.customLogLevelName);

        #endregion Internal Members

        #region Public Methods

        #region Log

        public static void LogA(this ILog log, object message) { _log(log, message, enLogLevel.Log, false, null); }
        public static void LogA(this ILog log, object message, bool includeStack) { _log(log, message, enLogLevel.Log, includeStack, null); }
        public static void LogA(this ILog log, object message, Exception exception) { _log(log, message, enLogLevel.Log, false, exception); }
        public static void LogA(this ILog log, object message, bool includeStack, Exception exception) { _log(log, message, enLogLevel.Log, includeStack, exception); }

        #endregion Log

        #region Debug

        public static void DebugA(this ILog log, object message) { _log(log, message, enLogLevel.Debug, false, null); }
        public static void DebugA(this ILog log, object message, bool includeStack) { _log(log, message, enLogLevel.Debug, includeStack, null); }
        public static void DebugA(this ILog log, object message, Exception exception) { _log(log, message, enLogLevel.Debug, false, exception); }
        public static void DebugA(this ILog log, object message, bool includeStack, Exception exception) { _log(log, message, enLogLevel.Debug, includeStack, exception); }

        #endregion Debug

        #region Fatal

        public static void FatalA(this ILog log, object message) { _log(log, message, enLogLevel.Fatal, false, null); }
        public static void FatalA(this ILog log, object message, bool includeStack) { _log(log, message, enLogLevel.Fatal, includeStack, null); }
        public static void FatalA(this ILog log, object message, Exception exception) { _log(log, message, enLogLevel.Fatal, false, exception); }
        public static void FatalA(this ILog log, object message, bool includeStack, Exception exception) { _log(log, message, enLogLevel.Fatal, includeStack, exception); }

        #endregion Fatal

        #region Info

        public static void InfoA(this ILog log, object message) { _log(log, message, enLogLevel.Info, false, null); }
        public static void InfoA(this ILog log, object message, bool includeStack) { _log(log, message, enLogLevel.Info, includeStack, null); }
        public static void InfoA(this ILog log, object message, Exception exception) { _log(log, message, enLogLevel.Info, false, exception); }
        public static void InfoA(this ILog log, object message, bool includeStack, Exception exception) { _log(log, message, enLogLevel.Info, includeStack, exception); }

        #endregion Info

        #region Warn

        public static void WarnA(this ILog log, object message) { _log(log, message, enLogLevel.Warn, false, null); }
        public static void WarnA(this ILog log, object message, bool includeStack) { _log(log, message, enLogLevel.Warn, includeStack, null); }
        public static void WarnA(this ILog log, object message, Exception exception) { _log(log, message, enLogLevel.Warn, false, exception); }
        public static void WarnA(this ILog log, object message, bool includeStack, Exception exception) { _log(log, message, enLogLevel.Warn, includeStack, exception); }

        #endregion Info

        #endregion Public Methods

        #region Logging Logic

        private static void _log(ILog log, object _message, enLogLevel _logLevel = enLogLevel.Log, bool _includeStack = false, Exception _exception = null)
        {
            var stackTrace = new StackTrace(_includeStack);
            var frame = stackTrace.GetFrame(1);
            dynamic container = new ExpandoObject();
            var mbase = frame.GetMethod();
            var functionAttributes = CustomAttributeData.GetCustomAttributes(mbase);
            var classAttributes = CustomAttributeData.GetCustomAttributes(mbase.ReflectedType);
            var props = typeof(LogCategoryAttribute).GetProperties();

            ParseCustomAttributes(functionAttributes, props, container);
            ParseCustomAttributes(classAttributes, props, container, "");

            container.BodyMessage = _message;

            if (_includeStack)
            {
                container.CallingMethod = mbase.Name;
                container.CallLocation = mbase.DeclaringType.FullName;
                container.StackTrace = prepareStackTrace(stackTrace);
            }
            var logMessage = JsonConvert.SerializeObject(container);

            switch (_logLevel)
            {
                case enLogLevel.Log:
                    log.Logger.Log(mbase.DeclaringType, logLevel, logMessage, null);
                    break;
                case enLogLevel.Debug:
                    log.Debug(logMessage, _exception);
                    break;
                case enLogLevel.Error:
                    log.Error(logMessage, _exception);
                    break;
                case enLogLevel.Fatal:
                    log.Fatal(logMessage, _exception);
                    break;
                case enLogLevel.Info:
                    log.Info(logMessage, _exception);
                    break;
                case enLogLevel.Warn:
                    log.Warn(logMessage, _exception);
                    break;
                default:
                    break;
            }

            
        }

        #endregion Logging Logic

        #region Private Methods


        private static string prepareStackTrace(StackTrace stackTrace)
        {
            StringBuilder trace = new StringBuilder();
            foreach (var frame in stackTrace.GetFrames())
            {
                if (frame.GetFileLineNumber() > 0)
                {
                    trace.AppendLine(string.Format("Filename: {0} Method: {1} Line: {2} Column: {3}  ",
                        frame.GetFileName(),
                        frame.GetMethod(),
                        frame.GetFileLineNumber(),
                        frame.GetFileColumnNumber()));
                }
            }
            return trace.ToString();
        }

        private static void AddProperty(ExpandoObject container,
                                        string propertyName,
                                        object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = container as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }

        private static void ParseCustomAttributes(
                            IList<CustomAttributeData> attributes,
                            PropertyInfo[] props,
                            ExpandoObject container, string preName = null)
        {
            for (var i = 0; i < attributes.Count; i++)
            {
                CustomAttributeData cad = attributes[i];
                string name = "";

                for (var j = 0; j < cad.ConstructorArguments.Count; j++)
                {
                    CustomAttributeTypedArgument cata = cad.ConstructorArguments[j];
                    if (cad.AttributeType.Name == "RouteAttribute")
                    {
                        name = "Route";
                    }
                    else
                    {
                        name = props[j].Name;
                    }
                    ParseAttributeData(cata, name, container, preName);
                }

                foreach (CustomAttributeNamedArgument cana
                    in cad.NamedArguments)
                {
                    ParseAttributeData(cana.TypedValue, cana.MemberName, container, preName);
                }
            }
        }

        private static void ParseAttributeData(CustomAttributeTypedArgument cata,
                                    string name,
                                    ExpandoObject container,
                                    string parentName)
        {
            if (cata.Value.GetType() == typeof(ReadOnlyCollection<CustomAttributeTypedArgument>))
            {
                foreach (CustomAttributeTypedArgument cataElement in
                    (ReadOnlyCollection<CustomAttributeTypedArgument>)cata.Value)
                {
                    if (cataElement.Value is string)
                    {
                        var nameValue = cataElement.Value.ToString().Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                        AddProperty(container, GetLogName(nameValue[0], parentName), nameValue[1]);
                    }
                    else
                    {
                        AddProperty(container, GetLogName(name, parentName), cataElement.Value);
                    }
                }
            }
            else
            {
                AddProperty(container, GetLogName(name, parentName), cata.Value);
            }
        }

        private static string GetLogName(string name, string parentName)
        {
            if (!string.IsNullOrEmpty(parentName))
            {
                return string.Concat(parentName, "-", name);
            }
            return name;
        }

        #endregion Private Methods
    }
}
