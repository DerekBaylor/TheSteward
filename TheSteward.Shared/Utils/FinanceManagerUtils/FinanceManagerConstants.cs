namespace TheSteward.Shared.Utils.FinanceManagerUtils;

public static class FinanceManagerConstants
{
    public enum FrequencyEnum
    {
        Monthly = 12,
        BiMonthly = 24,
        BiWeekly = 26,
        Weekly = 52,
    }

    public static readonly Guid StarterFinanceManagerId = Guid.Parse("273f5120-7e60-4b15-885d-eb45b0ad513a");

    public static readonly Guid StarterBudgetId = Guid.Parse("dc8014a8-9609-4353-b847-a10f6da88f81");

    public static readonly Guid StarterHousingCategoryId = Guid.Parse("579951c9-fdac-4d69-bb0e-5fd2d1223da8");
    public static readonly Guid StarterUtilityCategoryId = Guid.Parse("c3b60510-d5dd-4d96-b371-66527381b5c6");
    public static readonly Guid StarterTransportationCategoryId = Guid.Parse("33b36c8a-d954-4a3b-bdad-1ba5dc1b4768");
    public static readonly Guid StarterLivingCategoryId = Guid.Parse("e9cfb19d-68f1-42e6-939e-0340c1b19094");
    public static readonly Guid StarterCreditsCategoryId = Guid.Parse("2ea906a0-4abd-4d31-a547-41c5566f5160");
    public static readonly Guid StarterInvestmentsCategoryId = Guid.Parse("198e1ec7-30ee-45b5-868b-3d9ced59ac6d");
    public static readonly Guid StarterEntertainmentCategoryId = Guid.Parse("db110b69-15dd-4b36-87cc-4488b7579332");
}