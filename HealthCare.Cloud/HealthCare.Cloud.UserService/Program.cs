using HealthCare.Cloud.UserService.Data;
using HealthCare.Cloud.UserService.Repository;
using HealthCare.Common.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

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

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Db Context
builder.Services.AddPostgresDbContext<UserDbContext>(builder.Configuration, "HealthCareBookingSystemDB");

// Repositories
builder.Services.AddScoped<IUserDataRepository, UserDataRepository>();

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
