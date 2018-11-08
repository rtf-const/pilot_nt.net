using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotNtSharp
{
    public class CloseShiftResponse
    {
        public bool IsSuccess => Checks != null;

        /// <summary>
        /// Образ чека, который необходимо необходимо отправить на печать 
        /// (в нефискальном режиме) перед завершением транзакции
        /// </summary>
        public string[] Checks { get; internal set; }
    }
}
