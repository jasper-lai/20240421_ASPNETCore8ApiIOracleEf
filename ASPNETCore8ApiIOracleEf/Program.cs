using ASPNETCore8ApiIOracleEf.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using ASPNETCore8ApiIOracleEf.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

#region �[�J Oracle ��Ʈw�s���A�� �� DI
builder.Services.AddDbContext<HrDbContext>(
    options => options.UseOracle(builder.Configuration.GetConnectionString("HrConnection")));
#endregion

//#region �վ� AddSwaggerGen() ���ﶵ, �Ϩ�i�HŪ�� Xml Document
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "�H�O�귽�t�� Web API", Version = "v1" });
//    // Set the comments path for the Swagger JSON and UI.
//    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
//    c.IncludeXmlComments(xmlPath);
//});
//#endregion

#region �N AddOpenApiDocument() �A��, ���U�� DI
// Add OpenAPI v3 document
builder.Services.AddOpenApiDocument(settings =>
{
    settings.Title = "�H�O�귽�t�� Web API";
    settings.Version = "v1";
    settings.Description = "�H�O�귽�t��: ���ѤH���γ����޲z���\��";
});

// Add Swagger v2 document
// services.AddSwaggerDocument();
#endregion

#region ���U�M�׬����� services
builder.Services.AddScoped<IEmployeesService, EmployeesService>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    //#region �ҥ� Swagger ������ middleware
    ////app.UseSwagger();
    ////app.UseSwaggerUI();
    //#endregion

    #region �ҥ� NSwag ������ middleware
    // Add OpenAPI 3.0 document serving middleware
    // Available at: http://localhost:<port>/swagger/v1/swagger.json
    app.UseOpenApi();

    // Add web UIs to interact with the document
    // Available at: http://localhost:<port>/swagger
    app.UseSwaggerUi();
    #endregion

    #region �ҥ� ReDoc ������ middleware
    // ��1: ReDoc �O�b NSwag.AspNetCore �� NSwagApplicationBuilderExtensions ���O��
    // ��2: ReDoc ��������ɯ�ϱo�d��S�w���I, �ҫ��M���ɳ�����[���[�M�ֳt
    //     ���S�����ش��ժ��\��, �º����˵�
    app.UseReDoc(config =>  // serve ReDoc UI
    {
        // �o�̪� Path �Ψӳ]�w ReDoc UI ������ (���}���|) (�@�w�n�H / �׽u�}�Y)
        // Available at: http://localhost:<port>/redoc
        config.Path = "/redoc";
    }); 
    #endregion
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
