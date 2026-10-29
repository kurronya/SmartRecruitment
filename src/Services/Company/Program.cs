using Microsoft.EntityFrameworkCore;
using SmartRecruitment.Application.Interfaces;
using SmartRecruitment.Application.UseCases.Company.ApproveCompany;
using SmartRecruitment.Application.UseCases.Company.GetCompanyVerificationStatus;
using SmartRecruitment.Application.UseCases.Company.RegisterCompany;
using SmartRecruitment.Application.UseCases.Company.RejectCompany;
using SmartRecruitment.Infrastructure.Persistence;
using SmartRecruitment.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CompanyDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();

builder.Services.AddScoped<RegisterCompanyHandler>();
builder.Services.AddScoped<ApproveCompanyHandler>();
builder.Services.AddScoped<RejectCompanyHandler>();
builder.Services.AddScoped<GetCompanyVerificationStatusHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Internal endpoints (/internal/*) are only meant for other services, never for the public.
// When InternalApiKey is configured, callers must send it in the X-Internal-Key header.
var internalKey = app.Configuration["InternalApiKey"];
app.Use(async (context, next) =>
{
    if (!string.IsNullOrEmpty(internalKey)
        && context.Request.Path.StartsWithSegments("/internal")
        && context.Request.Headers["X-Internal-Key"] != internalKey)
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return;
    }

    await next();
});

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapControllers();

app.Run();
