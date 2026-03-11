namespace TheSteward.Core.Utils.FinanceManagerUtils;

public static class FinanceManagerConstants
{
    /// <summary>
    /// Represents the number of periods per year.
    ///     <list type="bullet">
    ///     <item><description><b>Monthly (12)</b> — Paid once per month</description></item>
    ///     <item><description><b>BiMonthly (24)</b> — Paid twice per month</description></item>
    ///     <item><description><b>BiWeekly (26)</b> — Paid every two weeks</description></item>
    ///     <item><description><b>Weekly (52)</b> — Paid every week</description></item>
    /// </list>
    /// </summary>
    public enum FrequencyEnum
    {
        Monthly = 12,
        BiMonthly = 24,
        BiWeekly = 26,
        Weekly = 52,
    }

    #region Taxes 

    /// <summary>
    /// Represents the IRS tax filing status used to determine applicable federal
    /// tax brackets when calculating.
    /// </summary>
    public enum FilingStatusEnum
    {
        Single,
        MarriedJointly,
        MarriedSeparately,
        HeadOfHousehold
    }

    public class TaxBracket
    {
        public decimal MinIncome { get; set; }
        public decimal MaxIncome { get; set; }
        public decimal Rate { get; set; }
    }

    #endregion Taxes
}