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

#region 加入 Oracle 資料庫連結服務 至 DI
builder.Services.AddDbContext<HrDbContext>(
    options => options.UseOracle(builder.Configuration.GetConnectionString("HrConnection")));
#endregion

//#region 調整 AddSwaggerGen() 的選項, 使其可以讀取 Xml Document
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "人力資源系統 Web API", Version = "v1" });
//    // Set the comments path for the Swagger JSON and UI.
//    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
//    c.IncludeXmlComments(xmlPath);
//});
//#endregion

#region 將 AddOpenApiDocument() 服務, 註冊至 DI
// Add OpenAPI v3 document
builder.Services.AddOpenApiDocument(settings =>
{
    settings.Title = "人力資源系統 Web API";
    settings.Version = "v1";
    settings.Description = "人力資源系統: 提供人員及部門管理的功能";
});

// Add Swagger v2 document
// services.AddSwaggerDocument();
#endregion

#region 註冊專案相關的 services
builder.Services.AddScoped<IEmployeesService, EmployeesService>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    //#region 啟用 Swagger 相關的 middleware
    ////app.UseSwagger();
    ////app.UseSwaggerUI();
    //#endregion

    #region 啟用 NSwag 相關的 middleware
    // Add OpenAPI 3.0 document serving middleware
    // Available at: http://localhost:<port>/swagger/v1/swagger.json
    app.UseOpenApi();

    // Add web UIs to interact with the document
    // Available at: http://localhost:<port>/swagger
    app.UseSwaggerUi();
    #endregion

    #region 啟用 ReDoc 相關的 middleware
    // 註1: ReDoc 是在 NSwag.AspNetCore 的 NSwagApplicationBuilderExtensions 類別裡
    // 註2: ReDoc 的側邊欄導航使得查找特定端點, 模型和文檔部分更加直觀和快速
    //     它沒有內建測試的功能, 純粹文件檢視
    app.UseReDoc(config =>  // serve ReDoc UI
    {
        // 這裡的 Path 用來設定 ReDoc UI 的路由 (網址路徑) (一定要以 / 斜線開頭)
        // Available at: http://localhost:<port>/redoc
        config.Path = "/redoc";
    }); 
    #endregion
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
