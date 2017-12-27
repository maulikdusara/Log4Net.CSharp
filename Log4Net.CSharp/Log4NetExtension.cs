using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4Net.CSharp
{
    public static class Log4NetExtensions
    {
        static readonly log4net.Core.Level jeLevel = LogManager.GetRepository().LevelMap[Configurations.customLogLevelName];
        static readonly log4net.Core.Level authLevel = new log4net.Core.Level(250, "JEM");
        public static void JELog(this ILog log, string message)
        {
            var method = System.Reflection.MethodBase.GetCurrentMethod();
            var m = new StackTrace().GetFrame(1).GetMethod();

            var attr = String.Join(",", 
                        (from p in m.CustomAttributes.ToList()
                        where p.AttributeType.Name == "JELoggingAttribute"
                        select new
                        {
                            Value = String.Format("{0}={1}", p.NamedArguments.FirstOrDefault().TypedValue.Value, p.NamedArguments.LastOrDefault().TypedValue.Value)
                        }).ToList()
                    ).Replace("{","").Replace("}","").Replace("Value = ","").Trim();


            message = string.Format("{0}, MessageBody={1}", attr, message);
            log.Logger.Log(method.DeclaringType, authLevel, message, null);

        }

        public static void JELogFormat(this ILog log, string message, params object[] args)
        {

            string formattedMessage = string.Format(message, args);
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                authLevel, formattedMessage, null);
        }
    }

}
