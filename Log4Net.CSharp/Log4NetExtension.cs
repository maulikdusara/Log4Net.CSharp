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
        static readonly log4net.Core.Level logLevel = new log4net.Core.Level(Configurations.customLogLevel,Configurations.customLogLevelName);
        public static void LogAttr(this ILog log, string message)
        {
            var stackTrace = new StackTrace(true);
            var frame = stackTrace.GetFrame(1);
            dynamic container = new ExpandoObject();
            var mbase = frame.GetMethod();
            var functionAttributes = CustomAttributeData.GetCustomAttributes(mbase);
            var classAttributes = CustomAttributeData.GetCustomAttributes(mbase.ReflectedType);
            var props = typeof(LogAttribute).GetProperties();

            ParseCustomAttributes(functionAttributes, props, container);
            ParseCustomAttributes(classAttributes, props, container, "parent");

            container.CallingMethod = mbase.Name;
            container.Message = message;
            container.StackTrace = prepareStackTrace(stackTrace);

            var logMessage = JsonConvert.SerializeObject(container);

            log.Logger.Log(mbase.DeclaringType, logLevel, logMessage, null);

        }

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

        private static void AddProperty(ExpandoObject container, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = container as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }

        private static void ParseCustomAttributes(
        IList<CustomAttributeData> attributes, PropertyInfo[] props, ExpandoObject container, string preName = null)
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

        private static void ParseAttributeData(CustomAttributeTypedArgument cata, string name, ExpandoObject container, string parentName)
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
    }
}
