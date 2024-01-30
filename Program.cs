using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SMTS;
using SMTS.Entities;
using SMTS.Helper;
using SMTS.Service;
using SMTS.Service.IService;
using SMTS.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddIdentity<MyUser, IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultTokenProviders();

builder.Services.AddDbContext<MESDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MESConnection")));

builder.Services.AddIdentity<MyUser, IdentityRole>()
    .AddEntityFrameworkStores<MESDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//builder.Services.AddAutoMapper(typeof(Program));

// Register ContactService for dependency injection (After Automapper, Before AddCors) Enable controllers and other components that require it through dependency injection.
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IWorkCenterService, WorkCenterService>();
builder.Services.AddScoped<IUOMService, UOMService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IPartService, PartService>();
builder.Services.AddScoped<IPartUOMService, PartUOMService>();
builder.Services.AddScoped<IPartDTLPrefixBatchSNService, PartDTLPrefixBatchSNService>();
builder.Services.AddScoped<IPlanningJOService, PlanningJOService>();
builder.Services.AddScoped<IPIOTCounterService,PIOTCounterService>();
builder.Services.AddScoped<IPIOTMaintainanceService, PIOTMaintainanceService>();
builder.Services.AddScoped<IPIOTRunningService, PIOTRunningService>();

builder.Services.AddScoped<IJobOrderService, JobOrderService>();
builder.Services.AddScoped<IJobOrderOperationService, JobOrderOperationService>();
builder.Services.AddScoped<IJobOperationStatusService, JobOperationStatusService>();
builder.Services.AddScoped<IJoBOMService, JoBOMService>();

builder.Services.AddScoped<IStockOutInDTLService, StockOutInDTLService>();
builder.Services.AddScoped<IStockOutInService, StockOutInService>();

builder.Services.AddScoped<IWeightReadingService, WeightReadingService>();
builder.Services.AddScoped<IStockOutInPropertyService, StockOutInPropertyService>();
builder.Services.AddScoped<IStockJobOperationRelationService, StockJobOperationRelationService>();



//builder.Services.AddCors(option =>
//{
//    var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
//    option.AddDefaultPolicy(builder =>
//    {
//        builder.WithOrigins(allowedOrigins).AllowAnyMethod().AllowAnyHeader();
//    });
//});

builder.Services.AddCors(options =>
{
    var frontendURL = builder.Configuration.GetValue<string>("frontend_url");

    //option.AddDefaultPolicy(
    //    builder =>
    //    {
    //        builder.WithOrigins(frontendURL)
    //        .AllowAnyMethod()
    //        .AllowAnyHeader()
    //        .WithExposedHeaders(new string[] { "totalAmountOfRecords" });
    //    });

    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("http://192.168.1.102:2025")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsAdmin", policy => policy.RequireClaim("role", "admin"));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["keyjwt"])),
            ClockSkew = TimeSpan.Zero
        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Allow /swagger to be used for default URL
app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

//app.UseCors();
app.UseCors(builder => builder
    .WithOrigins(
        "http://localhost:3000",
        "http://localhost:3001",  // Allow this origin
        "http://localhost:3002"   // Also allow this origin
    )
    .AllowAnyMethod()             // Allow all methods, e.g. GET, PUT, POST, etc.
    .AllowAnyHeader()             // Allow all headers
    .AllowCredentials());         // Allow credentials, e.g. cookies, authorization headers, etc.


app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

// Add the default route here
app.MapGet("/", () => Results.Redirect("/swagger"));


app.Run();
