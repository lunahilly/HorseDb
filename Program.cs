using Horse;
using Horse.Models.Enums;
using Horse.Web;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("psql");

builder.Services.AddPooledDbContextFactory<HorseDbContext>((options) =>
{
	var builder2 = new NpgsqlDataSourceBuilder(connectionString);
	builder2.MapEnum<HorseWeightState>();
	builder2.MapEnum<State>();
	builder2.MapEnum<HorseSex>();
	builder2.MapEnum<HumanSex>();
	builder2.MapEnum<AppointmentType>();

	options.UseNpgsql(builder2.Build(), static (o) =>
	{
		o.MapEnum<HumanSex>("human_sex");
		o.MapEnum<State>("state");
		o.MapEnum<HorseWeightState>("horse_weight_state");
		o.MapEnum<HorseSex>("horse_sex");
		o.MapEnum<AppointmentType>("appointment_type");
	});
}, 16);

// Add services to the container.
builder.Services.AddRazorComponents()
	   .AddInteractiveServerComponents();

builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();
