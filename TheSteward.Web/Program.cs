using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheSteward.Core.IRepositories;
using TheSteward.Core.IServices;
using TheSteward.Core.Models;
using TheSteward.Core.Profiles;
using TheSteward.Infrastructure.Data;
using TheSteward.Infrastructure.Repositories;
using TheSteward.Infrastructure.Services;
using TheSteward.Shared.Interfaces;
using TheSteward.Shared.Services;
using TheSteward.Web.Components;
using TheSteward.Web.Components.Account;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRazorPages();

#region Services & Repositories
builder.Services.AddScoped<INavigationService, NavigationService>();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IHouseholdRepository, HouseholdRepository>();
builder.Services.AddScoped<IUserHouseholdRepository, UserHouseholdRepository>();
builder.Services.AddScoped<IInvitationRepository, InvitationRepository>();

builder.Services.AddScoped<IHouseholdService, HouseholdService>();
builder.Services.AddScoped<IUserHouseholdService, UserHouseholdService>();

builder.Services.AddSingleton<HouseholdState>();

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
