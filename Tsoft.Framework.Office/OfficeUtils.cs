using System;
using System.Collections.Generic;
using System.Text;

namespace Tsoft.Framework.Office
{
    public class OfficeUtils
    {
        public static string Sum(string from, string to)
        {
            return "=SUM(" + from + ":" + to + ")";
        }

        public static string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }


        public static string GetRange(int fromCol, int toCol, int fromRow, int toRow)
        {
            return GetExcelColumnName(fromCol) + fromRow + ":" + GetExcelColumnName(toCol) + toRow;
        }
    }
}
