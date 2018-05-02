using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace homeFinanceMVC.Models
{
    public class Log
    {
        int idLog;
        string logDesc;
        DateTime fechaLog;

        public Log() { }

        public Log(string logDesc, DateTime fechaLog)
        {
            this.logDesc = logDesc;
            this.fechaLog = fechaLog;
        }

        public Log(string logDesc)
        {
            this.logDesc = logDesc;
        }
        public int IdLog
        {
            get
            {
                return idLog;
            }

            set
            {
                idLog = value;
            }
        }

        public string LogDesc
        {
            get
            {
                return logDesc;
            }

            set
            {
                logDesc = value;
            }
        }

        public DateTime FechaLog
        {
            get
            {
                return fechaLog;
            }

            set
            {
                fechaLog = value;
            }
        }
    }
}