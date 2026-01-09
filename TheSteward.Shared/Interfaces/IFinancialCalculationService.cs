namespace TheSteward.Shared.Interfaces;

public interface IFinancialCalculationService
{
    /// <summary>
    /// Calculates the future value of an account.
    /// </summary>
    /// <param name="p">Contribution Amount</param>
    /// <param name="r">Annual Interest Rate</param>
    /// <param name="n">Contribution Frequency</param>
    /// <param name="p0">Initial Principal Balance</param>
    /// <returns>double</returns>
    public double CalculateFutureValue(double p, double r, int n, double p0);


    // Simple Monthly Interest Calculation (No Compounding)
    public double CalculateSimpleMonthlyInterest(double balance, double apr);

    // Compounded Monthly Interest Calculation
    public double CalculateCompoundedMonthlyInterest(double balance, double apr);

    // Simple Interest Calculation
    public double CalculateSimpleYearlyInterest(double balance, double apr);

    // Compounded Monthly Interest Calculation
    public double CalculateCompoundedYearlyInterest(double balance, double apr);
}
