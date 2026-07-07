using MediatR;
using Infrastructure;
using API.Middleware;
using Serilog;
using Infrastructure.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();
builder.Host.UseDefaultServiceProvider(options=>
{
   options.ValidateScopes=true;
   options.ValidateOnBuild=true; 
});
// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
// Add MediatR from Application layer
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Application.Orders.Commands.CreateOrderCommand).Assembly));

// Add Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseTenantMiddleware(); 
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHub<OrderHub>("/orderHub");
// Run migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<Infrastructure.Data.ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();