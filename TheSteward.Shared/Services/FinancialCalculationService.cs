using TheSteward.Shared.Interfaces;

namespace TheSteward.Shared.Services;

internal class FinancialCalculationService : IFinancialCalculationService
{
    public double CalculateFutureValue(double p, double r, int n, double p0)
    {
        return p * ((Math.Pow(1 + r, n) - 1) / r) * (1 + r) + p0 * Math.Pow(1 + r, n);
    }

    public double CalculateSimpleMonthlyInterest(double balance, double apr)
    {
        double monthlyRate = apr / 100 / 12;
        return balance * monthlyRate;
    }

    public double CalculateCompoundedMonthlyInterest(double balance, double apr)
    {
        double monthlyRate = apr / 100 / 12;
        double newBalance = balance * (1 + monthlyRate);
        return newBalance - balance; // Only the interest portion
    }

    public double CalculateSimpleYearlyInterest(double balance, double apr)
    {
        return balance * (apr / 100);
    }

    public double CalculateCompoundedYearlyInterest(double balance, double apr)
    {
        double monthlyRate = apr / 100 / 12; // APR divided into monthly rate
        double amountAfterOneYear = balance * Math.Pow(1 + monthlyRate, 12);

        return amountAfterOneYear - balance; // Total interest paid
    }
}
