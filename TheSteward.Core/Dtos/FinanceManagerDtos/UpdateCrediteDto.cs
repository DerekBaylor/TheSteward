using System.ComponentModel.DataAnnotations;

namespace TheSteward.Core.Dtos.FinanceManagerDtos;

public class UpdateCreditDto
{
    [Required]
    public Guid CreditId { get; set; }
    
   [Required]
    [MaxLength(200)]
    public required string CreditName { get; set; }
    
    [MaxLength(50)]
    public string? CreditType { get; set; }
    
    [Required]
    [Range(0, 1, ErrorMessage = "Interest rate must be between 0 and 1 (e.g., 0.0725 for 7.25%)")]
    public decimal InterestRate { get; set; }
    
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Current value must be greater than or equal to 0")]
    public decimal CurrentValue { get; set; }
    
    /// <summary>
    /// Calculated on frontend before submission
    /// </summary>
    [Required]
    public decimal EstMonthlyInterest { get; set; }
    
    /// <summary>
    /// Calculated on frontend before submission
    /// </summary>
    [Required]
    public decimal EstYearlyInterest { get; set; }

    /// <summary>
    /// Monthly = 12, Bi-Monthly = 24, Bi-Weekly = 26, Weekly = 52
    /// </summary>
    [Required]
    public int PaymentFrequency { get; set; }
    
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Payment amount must be greater than or equal to 0")]
    public decimal PaymentAmount { get; set; }

    /// <summary>
    /// Day of the month (1-31). Use 31 for "last day of month"
    /// </summary>
    [Required]
    [Range(1, 31, ErrorMessage = "Payment day must be between 1 and 31")]
    public int PaymentDay { get; set; }
    
    public int DisplayOrder { get; set; }

    [Required]
    public Guid BudgetId { get; set; }
    
    public Guid? ExpenseId { get; set; }
}