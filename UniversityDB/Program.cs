// 1.- Using to Work with Entity Framwork
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UniversityDB;
using UniversityDB.DataAccess;
using UniversityDB.Services;

var builder = WebApplication.CreateBuilder(args);

// 2.- Connection with SQL Server 
const string CONNECTION_NAME = "UniversityBD";
var connectionString = builder.Configuration.GetConnectionString(CONNECTION_NAME);


// 3.- Add Context to Services of builder
builder.Services.AddDbContext<UniversityDBContext>(options => options.UseSqlServer(connectionString));

// 7.- Add Services of JWT Autorization
builder.Services.AddJwtTokenServices(builder.Configuration);

// Add services to the container.

builder.Services.AddControllers();

// 4.- Add Custom Services (from folder Services)
builder.Services.AddScoped<IStudentsService, StudentsService>();

// 8.- Add Authorization 
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserOnlyPolicy", policy => policy.RequireClaim("UserOnly", "User1"));
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// 9.- Config Swagger to take care of Autorization of JWT
builder.Services.AddSwaggerGen(options =>
    {
        // We define the Security for Authorization
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization Header using Bearer Scheme"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[]{}
            }
        });
    }
);

// 5.- CORS configuration
builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: "CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            }
        );
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// 6.- Tell app to use CORS
app.UseCors("CorsPolicy");

app.Run();
