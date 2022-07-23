using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace IndexValReaderUtility
{
    public static class Helper
    {
        public static string ToOracleDateFormat(this string cString)
        {
            DateTime output;
            DateTime checkedDate;
            if (DateTime.TryParse(cString, out checkedDate))
            {
                string[] formats = { "M/d/yyyy", "dd-MMM-yyyy", "dd/M/yyyy", "d/M/yyyy", "M-d-yyyy", "d-M-yyyy", "d-MMM-yy", "d-MMMM-yyyy", "yyyyMMdd", "yyyy-MM-dd", "yyyy/MM/dd", "yyyyMMMdd", "yyyy/MMM/dd", "yyyy-MMM-dd", "dd/MM/yyyy", "MM/dd/yyyy", "MM/dd/yyyy HH:mm:ss", "dd/MM/yyyy HH:mm:ss" };

                DateTime.TryParseExact(checkedDate.Date.ToString("dd-MMM-yyyy"), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out output);
                return output.ToString("dd-MMM-yyyy");

            }
            else
                return "";
        }

    }
}
