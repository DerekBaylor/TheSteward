using TheSteward.Shared.Interfaces;

namespace TheSteward.Shared.Services;

public class FinancialCalculationService : IFinancialCalculationService
{

    #region Investment Calculations
    
    public decimal CalculateFutureValue(decimal p, decimal r, int n, decimal p0)
    {
        // Convert to double for Math.Pow, then back to decimal
        decimal factor = (decimal)Math.Pow((double)(1 + r), n);
        return p * ((factor - 1) / r) * (1 + r) + p0 * factor;
    }

    #endregion Investment Calculations
    
    #region Simple Interest Calculations
    
    public decimal CalculateSimpleMonthlyInterest(decimal balance, decimal apr)
    {
        decimal monthlyRate = apr / 100m / 12m;
        return balance * monthlyRate;
    }

    public decimal CalculateCompoundedMonthlyInterest(decimal balance, decimal apr)
    {
        decimal monthlyRate = apr / 100m / 12m;
        decimal newBalance = balance * (1m + monthlyRate);
        return newBalance - balance;
    }

    public decimal CalculateSimpleYearlyInterest(decimal balance, decimal apr)
    {
        return balance * (apr / 100m);
    }

    public decimal CalculateCompoundedYearlyInterest(decimal balance, decimal apr)
    {
        decimal monthlyRate = apr / 100m / 12m;
        decimal amountAfterOneYear = balance * (decimal)Math.Pow((double)(1m + monthlyRate), 12);
        return amountAfterOneYear - balance;
    }
    
    #endregion  Simple Interest Calculations
    
    #region Credit-Specific Calculations

    public decimal CalculateCreditMonthlyInterest(decimal currentBalance, decimal interestRate)
    {
        return Math.Round(currentBalance * interestRate / 12m, 2);
    }

    public decimal CalculateCreditYearlyInterest(decimal currentBalance, decimal interestRate)
    {
        return Math.Round(currentBalance * interestRate, 2);
    }

    public decimal CalculateCreditMonthlyInterestCompounded(decimal currentBalance, decimal interestRate)
    {
        decimal dailyRate = interestRate / 365m;
        decimal amountAfterMonth = currentBalance * (decimal)Math.Pow((double)(1m + dailyRate), 30);
        return Math.Round(amountAfterMonth - currentBalance, 2);
    }

    public decimal CalculateCreditYearlyInterestCompounded(decimal currentBalance, decimal interestRate)
    {
        decimal dailyRate = interestRate / 365m;
        decimal amountAfterYear = currentBalance * (decimal)Math.Pow((double)(1m + dailyRate), 365);
        return Math.Round(amountAfterYear - currentBalance, 2);
    }
    
    #endregion Credit-specific calculations
    
    #region Payoff calculations

    public int CalculateMonthsToPayOff(decimal balance, decimal monthlyPayment, decimal interestRate)
    {
        decimal monthlyRate = interestRate / 12m;
        decimal monthlyInterest = balance * monthlyRate;

        // Check if payment is sufficient
        if (monthlyPayment <= monthlyInterest)
            return -1;

        // Formula: log(P / (P - B * r)) / log(1 + r)
        // Where P = payment, B = balance, r = monthly rate
        double numerator = Math.Log((double)(monthlyPayment / (monthlyPayment - balance * monthlyRate)));
        double denominator = Math.Log((double)(1m + monthlyRate));

        return (int)Math.Ceiling(numerator / denominator);
    }
    
    public decimal CalculateTotalInterestPaid(decimal balance, decimal monthlyPayment, decimal interestRate)
    {
        int months = CalculateMonthsToPayOff(balance, monthlyPayment, interestRate);
        
        if (months == -1)
            return -1;

        decimal totalPaid = monthlyPayment * months;
        return Math.Round(totalPaid - balance, 2);
    }

    #endregion Payoff calculations
    
    #region Utility methods

    public decimal ConvertAprToDecimal(decimal apr)
    {
        return apr / 100m;
    }

    public decimal ConvertDecimalToApr(decimal decimalRate)
    {
        return decimalRate * 100m;
    }
    
    #endregion Utility methods
}

