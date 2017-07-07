using System;
using System.Text;

namespace SpellChecker
{
    public static class TypeExtensions
    {
        public static string AggregateExceptions(this Exception ex)
        {
            if (ex == null) return string.Empty;
            var sb = new StringBuilder();
            if (ex.InnerException != null)
            {
                sb.AppendLine(ex.InnerException.AggregateExceptions());
            }
            else
            {
                sb.AppendLine(ex.Message);
            }
            return sb.ToString();
        }
    }
}
