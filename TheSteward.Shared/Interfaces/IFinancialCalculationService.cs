namespace TheSteward.Shared.Interfaces;

public interface IFinancialCalculationService
{
    #region Investment Calculations
    /// <summary>
    /// Calculates the future value of an investment with regular contributions.
    /// </summary>
    /// <param name="p">Regular payment/contribution amount.</param>
    /// <param name="r">Interest rate per period (as decimal, e.g., 0.05 for 5%).</param>
    /// <param name="n">Number of periods.</param>
    /// <param name="p0">Initial principal/present value.</param>
    /// <returns>The future value of the investment.</returns>
    decimal CalculateFutureValue(decimal p, decimal r, int n, decimal p0);
    
    #endregion Investment Calculations
    
    #region Simple Interest Calculations
    
    /// <summary>
    /// Calculates simple monthly interest on a balance.
    /// </summary>
    /// <param name="balance">The current balance.</param>
    /// <param name="apr">Annual Percentage Rate as a percentage (e.g., 19.99 for 19.99%).</param>
    /// <returns>The simple monthly interest amount.</returns>
    /// <remarks>
    /// Formula: balance × (APR / 100 / 12)
    /// This does not compound - it's a straight calculation of one month's interest.
    /// </remarks>
    decimal CalculateSimpleMonthlyInterest(decimal balance, decimal apr);
        
    /// <summary>
    /// Calculates compounded monthly interest on a balance.
    /// </summary>
    /// <param name="balance">The current balance.</param>
    /// <param name="apr">Annual Percentage Rate as a percentage (e.g., 19.99 for 19.99%).</param>
    /// <returns>The interest amount for one month with compounding.</returns>
    /// <remarks>
    /// This calculates what the interest would be after compounding for one month.
    /// </remarks>
    decimal CalculateCompoundedMonthlyInterest(decimal balance, decimal apr);
    
    /// <summary>
    /// Calculates simple yearly interest on a balance.
    /// </summary>
    /// <param name="balance">The current balance.</param>
    /// <param name="apr">Annual Percentage Rate as a percentage (e.g., 19.99 for 19.99%).</param>
    /// <returns>The simple yearly interest amount.</returns>
    /// <remarks>
    /// Formula: balance × (APR / 100)
    /// This is a straight calculation without compounding.
    /// </remarks>
    decimal CalculateSimpleYearlyInterest(decimal balance, decimal apr);
    
    /// <summary>
    /// Calculates compounded yearly interest on a balance with monthly compounding.
    /// </summary>
    /// <param name="balance">The current balance.</param>
    /// <param name="apr">Annual Percentage Rate as a percentage (e.g., 19.99 for 19.99%).</param>
    /// <returns>The total interest accrued over one year with monthly compounding.</returns>
    /// <remarks>
    /// Formula: balance × (1 + monthlyRate)^12 - balance
    /// This compounds interest monthly for 12 months.
    /// </remarks>
    decimal CalculateCompoundedYearlyInterest(decimal balance, decimal apr);

    #endregion Simple Interest Calculations
    
    #region Credit-specific calculations
    
    /// <summary>
    /// Calculates estimated monthly interest for a credit account.
    /// </summary>
    /// <param name="currentBalance">The current balance on the credit account.</param>
    /// <param name="interestRate">The interest rate as a decimal (e.g., 0.1999 for 19.99%).</param>
    /// <returns>The estimated monthly interest amount.</returns>
    /// <remarks>
    /// This uses simple interest calculation: balance × interestRate / 12
    /// For more accurate credit card interest, use <see cref="CalculateCreditMonthlyInterestCompounded"/>.
    /// </remarks>
    decimal CalculateCreditMonthlyInterest(decimal currentBalance, decimal interestRate);
    
    /// <summary>
    /// Calculates estimated yearly interest for a credit account.
    /// </summary>
    /// <param name="currentBalance">The current balance on the credit account.</param>
    /// <param name="interestRate">The interest rate as a decimal (e.g., 0.1999 for 19.99%).</param>
    /// <returns>The estimated yearly interest amount.</returns>
    /// <remarks>
    /// This uses simple interest calculation: balance × interestRate
    /// For more accurate credit card interest, use <see cref="CalculateCreditYearlyInterestCompounded"/>.
    /// </remarks>
    decimal CalculateCreditYearlyInterest(decimal currentBalance, decimal interestRate);
    
    /// <summary>
    /// Calculates estimated monthly interest for a credit account with daily compounding.
    /// </summary>
    /// <param name="currentBalance">The current balance on the credit account.</param>
    /// <param name="interestRate">The interest rate as a decimal (e.g., 0.1999 for 19.99%).</param>
    /// <returns>The estimated monthly interest amount with daily compounding.</returns>
    /// <remarks>
    /// Most credit cards compound daily. This is a more accurate calculation than simple interest.
    /// Formula: balance × (1 + dailyRate)^30 - balance
    /// Uses 30 days as average month length.
    /// </remarks>
    decimal CalculateCreditMonthlyInterestCompounded(decimal currentBalance, decimal interestRate);
    
    /// <summary>
    /// Calculates estimated yearly interest for a credit account with daily compounding.
    /// </summary>
    /// <param name="currentBalance">The current balance on the credit account.</param>
    /// <param name="interestRate">The interest rate as a decimal (e.g., 0.1999 for 19.99%).</param>
    /// <returns>The estimated yearly interest amount with daily compounding.</returns>
    /// <remarks>
    /// Most credit cards compound daily. This is a more accurate calculation than simple interest.
    /// Formula: balance × (1 + dailyRate)^365 - balance
    /// </remarks>
    decimal CalculateCreditYearlyInterestCompounded(decimal currentBalance, decimal interestRate);

    #endregion Credit-specific calculations
    
    #region Payoff calculations
        
    /// <summary>
    /// Calculates the time to pay off a credit balance.
    /// </summary>
    /// <param name="balance">The current balance.</param>
    /// <param name="monthlyPayment">The fixed monthly payment amount.</param>
    /// <param name="interestRate">The interest rate as a decimal (e.g., 0.1999 for 19.99%).</param>
    /// <returns>The number of months to pay off the balance, or -1 if payment is too low.</returns>
    /// <remarks>
    /// Returns -1 if the monthly payment is less than or equal to the monthly interest,
    /// as the balance would never be paid off.
    /// </remarks>
    int CalculateMonthsToPayOff(decimal balance, decimal monthlyPayment, decimal interestRate);
    
    /// <summary>
    /// Calculates the total interest paid over the life of a credit payoff.
    /// </summary>
    /// <param name="balance">The current balance.</param>
    /// <param name="monthlyPayment">The fixed monthly payment amount.</param>
    /// <param name="interestRate">The interest rate as a decimal (e.g., 0.1999 for 19.99%).</param>
    /// <returns>The total interest that will be paid, or -1 if payment is too low.</returns>
    decimal CalculateTotalInterestPaid(decimal balance, decimal monthlyPayment, decimal interestRate);

    #endregion Payoff calculations
    
    #region Utility methods
        
    /// <summary>
    /// Converts APR percentage to decimal rate.
    /// </summary>
    /// <param name="apr">Annual Percentage Rate as a percentage (e.g., 19.99).</param>
    /// <returns>The rate as a decimal (e.g., 0.1999).</returns>
    decimal ConvertAprToDecimal(decimal apr);
    
    /// <summary>
    /// Converts decimal rate to APR percentage.
    /// </summary>
    /// <param name="decimalRate">The rate as a decimal (e.g., 0.1999).</param>
    /// <returns>Annual Percentage Rate as a percentage (e.g., 19.99).</returns>
    decimal ConvertDecimalToApr(decimal decimalRate);
    
    #endregion Utility methods
}
