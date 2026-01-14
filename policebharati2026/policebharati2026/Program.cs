// using MasterApi.Services;
// using policebharati2026.Services;
// using Microsoft.OpenApi.Models;
// using policebharati2026.Data;
// using OfficeOpenXml;
// using policebharati2026.Services;

// var builder = WebApplication.CreateBuilder(args);

// ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

// // =======================
// // CONTROLLERS
// // =======================

// builder.Services.AddControllers()
//     .AddJsonOptions(opts =>
//     {
//         opts.JsonSerializerOptions.PropertyNamingPolicy =
//             System.Text.Json.JsonNamingPolicy.CamelCase;
//         opts.JsonSerializerOptions.DefaultIgnoreCondition =
//             System.Text.Json.Serialization.JsonIgnoreCondition.Never;
//     });

// // =======================
// // SERVICES (UNCHANGED)
// // =======================

// builder.Services.AddScoped<SqlHelper>();
// builder.Services.AddScoped<MasterService>();
// builder.Services.AddScoped<PetCandidateScoreService>();
// builder.Services.AddScoped<MasterBulkUploadService>();
// builder.Services.AddScoped<PhysicalStandardService>();
// builder.Services.AddScoped<LoginService>();

// // =======================
// // CORS (UPDATED, NOT REMOVED)
// // =======================

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowFrontend", policy =>
//     {
//         policy
//             .WithOrigins(
//                 "http://localhost:5173",   // React
//                 "http://localhost:5000",   // Swagger HTTP
//                 "https://localhost:5001"   // Swagger HTTPS
//             )
//             .AllowAnyHeader()
//             .AllowAnyMethod();
//     });
// });

// // =======================
// // SWAGGER (UNCHANGED)
// // =======================

// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(c =>
// {
//     c.SwaggerDoc("v1", new OpenApiInfo
//     {
//         Title = "Master & PET Candidate Score API",
//         Version = "v1"
//     });
// });



// var app = builder.Build();

// // =======================
// // MIDDLEWARE ORDER (CRITICAL FIX)
// // =======================

// // Swagger
// app.UseSwagger();
// app.UseSwaggerUI(c =>
// {
//     c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
//     c.RoutePrefix = string.Empty;
// });

// // ❗ Disable HTTPS redirect ONLY in Development
// if (!app.Environment.IsDevelopment())
// {
//     app.UseHttpsRedirection();
// }

// app.UseRouting();

// // ✅ CORS MUST BE BEFORE Authorization
// app.UseCors("AllowFrontend");

// app.UseAuthorization();

// app.MapControllers();

// app.Run();





using MasterApi.Services;
using policebharati2026.Services;
using Microsoft.OpenApi.Models;
using policebharati2026.Data;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

// =======================
// CONTROLLERS + JSON
// =======================

builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        // ✅ KEEP CAMEL CASE (matches React fetch + your current APIs)
        opts.JsonSerializerOptions.PropertyNamingPolicy =
            System.Text.Json.JsonNamingPolicy.CamelCase;

        opts.JsonSerializerOptions.DefaultIgnoreCondition =
            System.Text.Json.Serialization.JsonIgnoreCondition.Never;
    });

// =======================
// SERVICES (UNCHANGED)
// =======================

builder.Services.AddScoped<SqlHelper>();
builder.Services.AddScoped<MasterService>();
builder.Services.AddScoped<PetCandidateScoreService>();
builder.Services.AddScoped<MasterBulkUploadService>();
builder.Services.AddScoped<PhysicalStandardService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<ICandidateService, CandidateService>();



// =======================
// CORS (SAFE + REQUIRED)
// =======================

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


// =======================
// SWAGGER
// =======================

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Master & PET Candidate Score API",
        Version = "v1"
    });
});

var app = builder.Build();

// =======================
// MIDDLEWARE ORDER (IMPORTANT)
// =======================

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    c.RoutePrefix = string.Empty;
});

// ❗ Disable HTTPS redirect ONLY in Development
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseRouting();

// ✅ CORS MUST COME BEFORE Authorization
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
