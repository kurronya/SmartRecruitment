using Microsoft.EntityFrameworkCore;
using SmartRecruitment.Application.Interfaces;
using SmartRecruitment.Application.UseCases.Candidate.ChangeCandidateStatus;
using SmartRecruitment.Application.UseCases.Candidate.CreateCandidate;
using SmartRecruitment.Application.UseCases.Company.ApproveCompany;
using SmartRecruitment.Application.UseCases.Company.RegisterCompany;
using SmartRecruitment.Application.UseCases.Company.RejectCompany;
using SmartRecruitment.Application.UseCases.CV.FinalizeCV;
using SmartRecruitment.Application.UseCases.CV.GenerateCV;
using SmartRecruitment.Application.UseCases.CV.UpdateCVContent;
using SmartRecruitment.Application.UseCases.CV.UploadCV;
using SmartRecruitment.Application.UseCases.Interview.CancelInterview;
using SmartRecruitment.Application.UseCases.Interview.MarkInterviewCompleted;
using SmartRecruitment.Application.UseCases.Interview.RecordInterviewResult;
using SmartRecruitment.Application.UseCases.Interview.ScheduleInterview;
using SmartRecruitment.Application.UseCases.JobDescription.ApproveJobDescription;
using SmartRecruitment.Application.UseCases.JobDescription.PublishJobDescription;
using SmartRecruitment.Application.UseCases.JobDescription.RejectJobDescription;
using SmartRecruitment.Application.UseCases.JobDescription.StartJobDescriptionReview;
using SmartRecruitment.Application.UseCases.JobDescription.SubmitJobDescription;
using SmartRecruitment.Infrastructure.AI;
using SmartRecruitment.Infrastructure.Persistence;
using SmartRecruitment.Infrastructure.Persistence.Repositories;
using SmartRecruitment.Application.UseCases.Matching.MatchCandidatesForJobDescription;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SmartRecruitmentDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register each repository interface with its EF Core implementation -
// this is where the Application layer's abstractions get a real, concrete backing
builder.Services.AddScoped<ICandidateRepository, CandidateRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IJobDescriptionRepository, JobDescriptionRepository>();
builder.Services.AddScoped<IInterviewRepository, InterviewRepository>();
builder.Services.AddScoped<ICVRepository, CVRepository>();

// Register each Handler so ASP.NET Core can construct it and inject it into a Controller
builder.Services.AddScoped<CreateCandidateHandler>();
builder.Services.AddScoped<ChangeCandidateStatusHandler>();

builder.Services.AddScoped<RegisterCompanyHandler>();
builder.Services.AddScoped<ApproveCompanyHandler>();
builder.Services.AddScoped<RejectCompanyHandler>();

builder.Services.AddScoped<SubmitJobDescriptionHandler>();
builder.Services.AddScoped<StartJobDescriptionReviewHandler>();
builder.Services.AddScoped<ApproveJobDescriptionHandler>();
builder.Services.AddScoped<RejectJobDescriptionHandler>();
builder.Services.AddScoped<PublishJobDescriptionHandler>();

builder.Services.AddScoped<ScheduleInterviewHandler>();
builder.Services.AddScoped<MarkInterviewCompletedHandler>();
builder.Services.AddScoped<RecordInterviewResultHandler>();
builder.Services.AddScoped<CancelInterviewHandler>();

builder.Services.AddScoped<IEmbeddingService, FakeEmbeddingService>();
builder.Services.AddScoped<ICVTextExtractor, FakeCVTextExtractor>();
builder.Services.AddScoped<ICVContentGenerator, FakeCVContentGenerator>();

builder.Services.AddScoped<UploadCVHandler>();
builder.Services.AddScoped<GenerateCVHandler>();
builder.Services.AddScoped<UpdateCVContentHandler>();
builder.Services.AddScoped<FinalizeCVHandler>();

builder.Services.AddScoped<MatchCandidatesForJobDescriptionHandler>();

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

app.Run();
