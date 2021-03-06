﻿using System.Collections.Generic;
using System.Diagnostics;

namespace IST.Interfaces.IServices
{
    /// <summary>
    /// Logger interface
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Write to the log
        /// </summary>
        void Write(string message, string category, int priority, int eventId, TraceEventType severity, string title);

        /// <summary>
        /// Write to the log 
        /// </summary>
        void Write(object message, string category, int priority, int eventId, TraceEventType severity, string title, IDictionary<string, object> properties);
        //SearchTemplateResponse<IST.Models.LoggerModels.Log> SearchLogs(LogSearchRequest searchRequest);
    }
}
