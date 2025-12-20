using IXM.DB;
using IXM.DB.Services;
using Microsoft.EntityFrameworkCore;
using IXM.API.Services;
//using IXM.API;
using IXM.Common;
using IXM.Common.Data;
using IXM.DB.Server;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using IXM.GeneralSQL;
using Serilog;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using IXM.Models;
using IXM.Models.Log;
//using MassTransit;
using Microsoft.Extensions.Options;
using IXM.Constants;
using IXM.DB.Finance;
using Microsoft.AspNetCore.OData;

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));
builder.Services.AddOpenTelemetry()
    .ConfigureResource(res => res.AddService("IXMWeb"))

    .WithTracing(trace => { trace.AddHttpClientInstrumentation()
                                .AddAspNetCoreInstrumentation()
                                .AddSource();
                            trace.AddOtlpExporter(opt =>
                            {
                                opt.Endpoint = new Uri(builder.Configuration.GetValue<string>("Telemetry:TelemetryURITrace") ?? "http://localhost");
                                opt.Headers = builder.Configuration.GetValue<string>("Telemetry:TelemetryAPIKey");
                                opt.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;

                            });});

// The .WithTracing is currentyly not working...need more checking for functionality

//builder.Logging.ClearProviders();

/*Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.OpenTelemetry(x =>
        {
            x.Endpoint = "http://ixm-appsrv:5341/ingest/otlp/v1/logs";
            x.Protocol = Serilog.Sinks.OpenTelemetry.OtlpProtocol.HttpProtobuf;
           x.Headers = new Dictionary<string, string>
           { 
               ["X-Seq-ApiKey"] = "NDPAQ7MDz9ublGspOo1f"
           };
        })
    .CreateLogger();
*/
//builder.Logging.AddOpenTelemetry(x => x.AddConsoleExporter());
/*builder.Logging.AddOpenTelemetry(x =>
    {
        x.AddOtlpExporter(a =>
        {
            a.Endpoint = new Uri("http://ixm-appsrv:5341/ingest/otlp/v1/logs");
            a.Protocol = OtlpExportProtocol.HttpProtobuf;
            a.Headers = "X-Seq-ApiKey=NDPAQ7MDz9ublGspOo1f";
        });
        x.IncludeScopes = true;
        x.IncludeFormattedMessage = true;
    });
*/

builder.Services.AddCors(o => {
    o.AddPolicy("IXM_AllPolicy", builder =>
                             builder.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
                              });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddDbContext<IXMWriteDBContext>(o => o.UseFirebird(builder.Configuration.GetConnectionString("FirebirdData")));
builder.Services.AddDbContext<IXMDBContext>(o => o.UseFirebird(builder.Configuration.GetConnectionString("FirebirdData")));

builder.Services.AddDbContext<IXMDBIdentity>(o => o.UseFirebird(builder.Configuration.GetConnectionString("FirebirdMobi")));

builder.Services.AddDbContext<IXMAppDBContext>(o => o.UseFirebird(builder.Configuration.GetConnectionString("FirebirdCtrl")));

builder.Services.AddDbContext<IXMAppLogContext>(o => o.UseFirebird(builder.Configuration.GetConnectionString("FirebirdLogE")));

builder.Services.AddDbContext<IXMStoreDBContext>(o => o.UseFirebird(builder.Configuration.GetConnectionString("FirebirdStore")));
builder.Services.AddDbContext<IXMSystemDBContext>(o => o.UseFirebird(builder.Configuration.GetConnectionString("IXMSystem")));
builder.Services.AddDbContext<IXMSystemWrDBContext>(o => o.UseFirebird(builder.Configuration.GetConnectionString("IXMSystem")));
builder.Services.AddDbContext<IXMJobDBContext>(o => o.UseFirebird(builder.Configuration.GetConnectionString("FirebirdJob")));
builder.Services.AddMemoryCache();

builder.Services.AddSingleton<IXMDBContextFactory>();
builder.Services.AddSingleton<IXMWriteDBContextFactory>();

builder.Services.AddControllers().AddOData(option => { option.Count().Select().Filter().OrderBy().Expand(); });

builder.Services.AddTransient<IGeneralSQL, GeneralSQL>();
builder.Services.AddTransient<IDataValidator, DataValidator>();
builder.Services.AddSingleton<CustomData>();
builder.Services.AddScoped<IFinance, Finance>();
builder.Services.AddTransient<IIXMCommonRepo, IXMCommonRepo>();
builder.Services.AddTransient<IDataServicesServer, DataServicesServer>();  // From IXM.Common
builder.Services.AddTransient<IDataImport, DataImport>();

builder.Services.AddScoped<IIXMDBRepo, IXMDBRepo>();


builder.Services.AddScoped<IIXMTransactionRepo, IXMTransactionRepo>();
builder.Services.AddScoped<IIXMDocumentRepo, IXMDocumentRepo>();
//builder.Services.AddTransient<IFileService, FileService>();

builder.Services.AddTransient<IStoreService, StoreService>();
builder.Services.AddScoped<ICustomUpdates, CustomUpdates>();
builder.Services.AddTransient<IDataService, DataService>();






builder.Services.AddTransient<IQueryRepository, QueryRepository>();

builder.Services.AddAuthentication();

builder.Services.AddIdentityApiEndpoints <ApplicationUser> ()
                .AddRoles<IdentityRole>()    
                .AddEntityFrameworkStores<IXMDBIdentity>()
                .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(opts => { 
    opts.SignIn.RequireConfirmedEmail = false;
    opts.Password.RequiredLength = 5;
    opts.Password.RequireNonAlphanumeric = false;
});
builder.Services.Configure<DataProtectionTokenProviderOptions>(opts => opts.TokenLifespan = TimeSpan.FromHours(24));

builder.Services.Configure<AppRabbitMQ.MessageQBroker>(
    builder.Configuration.GetSection("MessageBroker"));

builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<AppRabbitMQ.MessageQBroker>>().Value);



/*
builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();
    busConfigurator.UsingRabbitMq((context, config) =>
    {
        AppRabbitMQ.MessageQBroker messageQBroker = context.GetRequiredService<AppRabbitMQ.MessageQBroker>();
        config.Host(new Uri(messageQBroker.Host), h =>
        {
            h.Username(messageQBroker.Username);
            h.Password(messageQBroker.Password);

        });
        config.ConfigureEndpoints(context);
        //config.Publish<RemittanceCreateEvent>(c => c.ExchangeType = ExchangeType.Fanout);
        //config.Publish<XLS_REMITTANCE>();
        config.UseMessageRetry(r => r.Immediate(3));
    });

    //busConfigurator.AddHostedService<RemittanceCreateEvent>();
    //busConfigurator.UsingInMemory((context, config) => config.ConfigureEndpoints(context));
});
*/


CommonConstants.ScheduleRowStart = Convert.ToInt16(builder.Configuration.GetConnectionString("ScheduleRowStart"));


//This seems to be an old of doing things
builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc(SwaggerConfiguration.DocNameV1,
                       new OpenApiInfo
                       {
                           Title = SwaggerConfiguration.DocInfoTitle,
                           Version = SwaggerConfiguration.DocInfoVersion,
                           Description = SwaggerConfiguration.DocInfoDescription
                       });

    var securitySchema = new OpenApiSecurityScheme
    {
        Description = SwaggerConfiguration.SecuritySchemeDescription,
        Name = SwaggerConfiguration.SecuritySchemeName,
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = SwaggerConfiguration.SecurityScheme.ToLower(),
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = SwaggerConfiguration.SecurityScheme
        }
    };
    swagger.AddSecurityDefinition(SwaggerConfiguration.SecurityScheme, securitySchema);

    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                            new string[] {}

                    }
                });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//}

app.MapIdentityApi<ApplicationUser>();

app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(builder.Configuration.GetConnectionString("BaseDocFolder")),
    RequestPath = "/APPFILES"
});

app.UseHttpsRedirection();

app.UseCors("IXM_AllPolicy");

app.UseAuthorization();

app.MapControllers();

app.UseSerilogRequestLogging();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
