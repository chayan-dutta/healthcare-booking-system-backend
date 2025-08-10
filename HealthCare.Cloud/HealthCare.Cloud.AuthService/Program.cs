using HealthCare.Cloud.AuthService.Data;
using HealthCare.Cloud.AuthService.ServiceClients;
using HealthCare.Common.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add CORS service - allow any origin
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPostgresDbContext<AuthServiceDbContext>(builder.Configuration, "HealthCareBookingSystemDB");

builder.Services.AddScoped<IUserServiceClient, UserServiceClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
