using CSharpFunctionalExtensions;

namespace Utility.Extensions;

public class RangeDate : ValueObject
{
    public RangeDate()
    {

    }
    public RangeDate(DateTime dateFrom, DateTime dateTo)
    {

    }

    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }

    public RangeDate GetFirsDayOfMonth()
    {
        var result = new RangeDate
        {
            DateFrom = DateFrom?.Date.FirstDayOfMonth(),
            DateTo = DateTo?.Date.FirstDayOfMonth()
        };


        return result;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return this.DateFrom;
        yield return this.DateTo;
    }
}