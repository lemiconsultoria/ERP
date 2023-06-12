namespace ERP.Report.Application.Queries
{
    public static class EntryBalanceFiltersHelper
    {
        public static List<string> IsValidDateRef(DateTime dataRef)
        {
            var errors = new List<string>();
            if (dataRef == DateTime.MinValue)
            {
                errors.Add("DataRef is invalid");
            }

            return errors;
        }

        public static List<string> IsValidPeriod(DateTime dataStart, DateTime dateEnd)
        {
            var errors = new List<string>();

            var errorStart = IsValidDateRef(dataStart);
            if (errorStart.Any())
                errors.AddRange(errorStart);

            var errorEnd = IsValidDateRef(dateEnd);
            if (errorEnd.Any())
                errors.AddRange(errorEnd);

            if (dataStart > dateEnd)
                errors.Add("dataStart is greater than dateEnd");

            return errors;
        }
    }
}