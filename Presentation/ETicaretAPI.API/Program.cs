using ETicaretAPI.Application.Validators.Products;
using ETicaretAPI.Infrastructure.Filters;
using ETicaretAPI.Infrastructure;
using ETicaretAPI.Application;
using ETicaretAPI.Persistence;
using FluentValidation.AspNetCore;
using ETicaretAPI.Infrastructure.Services.Storage.Local;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.PostgreSQL;
using System.Security.Claims;
using Serilog.Context;
using ETicaretAPI.API.Configurations.ColumnWriters;
using Microsoft.AspNetCore.HttpLogging;
using ETicaretAPI.API.Extensions;
using ETicaretAPI.SignalR;
using ETicaretAPI.API.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();//Client tan gelen request neticesinde oluşturulan HttpContext nesnesine katmanlardaki class lar üzerinden(business logic) erişebilmemizi sağlayan bir servistir.
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
builder.Services.AddSignalRServices();

builder.Services.AddStorage<LocalStorage>();
//builder.Services.AddStorage(ETicaretAPI.Infrastructure.Enums.StorageType.Local);

builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

Logger log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt")
    .WriteTo.PostgreSQL(builder.Configuration.GetConnectionString("PostgreSQL"), "logs",
    needAutoCreateTable: true,
        columnOptions: new Dictionary<string, ColumnWriterBase>
        {
            {"message",new RenderedMessageColumnWriter() },
            {"message_template",new MessageTemplateColumnWriter()},
            {"level",new LevelColumnWriter() },
            {"time_stamp",new TimestampColumnWriter() },
            {"exception",new ExceptionColumnWriter() },
            {"log_event",new LogEventSerializedColumnWriter()},
            {"user_name",new UsernameColumnWriter() }
        }
    )
    .WriteTo.Seq(builder.Configuration["Seq:ServerURL"])
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog(log);

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
    options.Filters.Add<RolePermissionFilter>();
})
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin", options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true,/* oluşturulan token değerini kimlerin/hangi originlerin/sitelerin kullanacağını belirlediğimiz değerdir. */
            ValidateIssuer = true,/* oluşturulacak token değerini kimin dağıttığını ifade edeceğimiz alandır. */
            ValidateLifetime = true,/* oluşturulan token değerinin süresini kontrol edecek olan doğrulamadır. */
            ValidateIssuerSigningKey = true,/* üretilecek token değerinin uygulamamıza ait bir değer olduğunu ifade eden security key verisinin doğrulamasıdır. */

            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,

            NameClaimType = ClaimTypes.Name//JWT üzerinde Name claimine karşılık gelen değeri User.Identity.Name propertysinden elde edebiliyoruz.
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler<Program>(app.Services.GetRequiredService<ILogger<Program>>());
app.UseStaticFiles();

app.UseSerilogRequestLogging();
app.UseHttpLogging();

app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    var username = context.User?.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;
    LogContext.PushProperty("user_name", username);
    await next();
});

app.MapControllers();
app.MapHubs();

app.Run();
