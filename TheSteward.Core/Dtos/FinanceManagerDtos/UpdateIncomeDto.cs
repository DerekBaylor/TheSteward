using System.ComponentModel.DataAnnotations;

namespace TheSteward.Core.Dtos.FinanceManagerDtos;

public class UpdateIncomeDto
{    
    [Required]
    public Guid IncomeId { get; set; }

    [Required]
    [MaxLength(200)]
    public required string IncomeName { get; set; }

    [Required]
    [Range(12, 52, ErrorMessage = "Income frequency must be 12, 24, 26, or 52")]
    public int IncomeFrequency { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "PayCheck gross must be greater than 0")]
    public decimal PayCheckGross { get; set; }

    public int DisplayOrder { get; set; }
}