using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheSteward.Core.IRepositories;
using TheSteward.Core.Models;
using TheSteward.Core.Profiles;
using TheSteward.Infrastructure.Data;
using TheSteward.Infrastructure.Repositories;
using TheSteward.Shared.Interfaces;
using TheSteward.Shared.Services;
using TheSteward.Web.Components;
using TheSteward.Web.Components.Account;
using MudBlazor.Services;
using TheSteward.Core.IRepositories.FinanceManagerIRepositories;
using TheSteward.Core.IRepositories.HouseholdIRepositories;
using TheSteward.Core.IServices.FinanceManagerIServices;
using TheSteward.Core.IServices.HouseholdIServices;
using TheSteward.Infrastructure.Repositories.FinanceManagerRepositories;
using TheSteward.Infrastructure.Repositories.HouseholdRepositories;
using TheSteward.Infrastructure.Services.FinanceManagerServices;
using TheSteward.Infrastructure.Services.HouseholdServices;
using TheSteward.Infrastructure.Repositories.TaskManagerRepositories;
using TheSteward.Core.IRepositories.ITaskManagerRepositories;
using TheSteward.Infrastructure.Services.TaskManagerServices;
using TheSteward.Core.IServices.TaskManagerIServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRazorPages();
builder.Services.AddMudServices();

#region Services & Repositories
builder.Services.AddScoped<INavigationService, NavigationService>();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IHouseholdRepository, HouseholdRepository>();
builder.Services.AddScoped<IUserHouseholdRepository, UserHouseholdRepository>();
builder.Services.AddScoped<IInvitationRepository, InvitationRepository>();

builder.Services.AddScoped<IHouseholdService, HouseholdService>();
builder.Services.AddScoped<IUserHouseholdService, UserHouseholdService>();

builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
builder.Services.AddScoped<IBudgetCategoryRepository, BudgetCategoryRepository>();
builder.Services.AddScoped<IBudgetSubCategoryRepository, BudgetSubCategoryRepository>();
builder.Services.AddScoped<ICreditRepository, CreditRepository>();
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<IIncomeRepository, IncomeRepository>();
builder.Services.AddScoped<IInvestmentRepository, InvestmentRepository>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<IBudgetCategoryService, BudgetCategoryService>();
builder.Services.AddScoped<IBudgetSubCategoryService, BudgetSubCategoryService>();
builder.Services.AddScoped<ICreditService, CreditService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IIncomeService, IncomeService>();
builder.Services.AddScoped<IInvestmentService, InvestmentService>();
builder.Services.AddScoped<IFinancialCalculationService, FinancialCalculationService>();

builder.Services.AddScoped<ITaskItemRepository, TaskItemRepository>();
builder.Services.AddScoped<ITaskItemOccurenceRepository, TaskItemOccurrenceRepository>();
builder.Services.AddScoped<ITaskItemCategoryRepository, TaskItemCategoryRepository>();
builder.Services.AddScoped<IRecurrenceRuleRepository, RecurrenceRuleRepository>();
builder.Services.AddScoped<IRecurrenceRuleService, RecurrenceRuleService>();
builder.Services.AddScoped<ITaskItemCategoryService, TaskItemCategoryService>();
builder.Services.AddScoped<ITaskItemService, TaskItemService>();
builder.Services.AddScoped<ITaskItemOccurrenceService, TaskItemOccurrenceService>();

builder.Services.AddScoped<HouseholdState>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
#endregion Services & Repositories

#region Automapper Profiles
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<HouseholdProfiles>();
    config.AddProfile<FinanceManagerProfiles>();
});
#endregion  Automapper Profiles

#region Auth
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;

    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+' ";
})
    .AddEntityFrameworkStores<TheStewardContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();
#endregion Auth

#region Connection Strings
builder.Services.AddDbContext<TheStewardContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
#endregion Connection Strings




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapPost("/account/logout", async (
    SignInManager<ApplicationUser> signInManager,
    HouseholdState householdState) =>
{
    householdState.ClearHousehold();
    await signInManager.SignOutAsync();
    return Results.Redirect("/");
}).RequireAuthorization();

app.Run();
