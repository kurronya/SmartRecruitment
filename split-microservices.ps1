# split-microservices.ps1
# Run from the repo root (next to SmartRecruitment.sln), on a NEW branch, with a clean working tree.
#   powershell -ExecutionPolicy Bypass -File .\split-microservices.ps1
# Requires split-new-files.zip in the same folder. Uses `git mv`, so file history is preserved.

$ErrorActionPreference = "Stop"

# ---------------------------------------------------------------- safety checks
if (-not (Test-Path "SmartRecruitment.sln")) { throw "Run this script from the repo root (next to SmartRecruitment.sln)." }
if (-not (Test-Path "split-new-files.zip")) { throw "split-new-files.zip must be in the repo root next to this script." }
if (Test-Path "SmartRecruitment.Application/UseCases/CV/GenerateCV") { throw "GenerateCV still exists. Finish the cleanup step (remove AI CV generation) first." }

$dirty = git status --porcelain | Where-Object { $_ -notmatch 'split-(microservices\.ps1|new-files\.zip)' }
if ($dirty) { throw "Working tree is not clean. Commit or stash your changes first:`n$dirty" }

# ---------------------------------------------------------------- helpers
$layerFolder = @{
    "SmartRecruitment.Domain"         = "Domain"
    "SmartRecruitment.Application"    = "Application"
    "SmartRecruitment.Infrastructure" = "Infrastructure"
    "SmartRecruitment.API"            = "Api"
}

function Move-Git([string]$src, [string]$dst) {
    $dir = Split-Path $dst -Parent
    if ($dir) { New-Item -ItemType Directory -Force -Path $dir | Out-Null }
    git mv $src $dst
    if ($LASTEXITCODE -ne 0) { throw "git mv failed: $src -> $dst" }
}

function Move-ToService([string]$service, [string[]]$paths) {
    foreach ($path in $paths) {
        $project = $path.Split("/")[0]
        $relative = $path.Substring($project.Length + 1)
        Move-Git $path "src/Services/$service/$($layerFolder[$project])/$relative"
    }
}

function Set-DbContextName([string]$service, [string]$contextName) {
    $dir = "src/Services/$service/Infrastructure/Persistence/Repositories"
    Get-ChildItem $dir -Filter *.cs | ForEach-Object {
        $text = [System.IO.File]::ReadAllText($_.FullName)
        $text = $text -replace 'SmartRecruitmentDbContext', $contextName
        [System.IO.File]::WriteAllText($_.FullName, $text, (New-Object System.Text.UTF8Encoding($true)))
    }
}

$D = "SmartRecruitment.Domain"
$A = "SmartRecruitment.Application"
$I = "SmartRecruitment.Infrastructure"
$W = "SmartRecruitment.API"

# ---------------------------------------------------------------- 1. Recruitment needs its own copy of the embedding port + fake adapter
#      (the originals move to the CV service below)
New-Item -ItemType Directory -Force -Path "src/Services/Recruitment/Application/Interfaces" | Out-Null
New-Item -ItemType Directory -Force -Path "src/Services/Recruitment/Infrastructure/AI" | Out-Null
Copy-Item "$A/Interfaces/IEmbeddingService.cs" "src/Services/Recruitment/Application/Interfaces/IEmbeddingService.cs"
Copy-Item "$I/AI/FakeEmbeddingService.cs" "src/Services/Recruitment/Infrastructure/AI/FakeEmbeddingService.cs"

# ---------------------------------------------------------------- 2. Move existing files into their services
Write-Host "Moving Candidate service files..."
Move-ToService "Candidate" @(
    "$D/Entities/Candidate.cs",
    "$D/Enums/CandidateStatus.cs",
    "$A/UseCases/Candidate",
    "$A/Interfaces/ICandidateRepository.cs",
    "$I/Persistence/Configurations/CandidateConfiguration.cs",
    "$I/Persistence/Repositories/CandidateRepository.cs",
    "$W/Controllers/CandidatesController.cs",
    "$W/Models/CreateCandidateRequest.cs",
    "$W/Models/ChangeCandidateStatusRequest.cs"
)

Write-Host "Moving Company service files..."
Move-ToService "Company" @(
    "$D/Entities/Company.cs",
    "$D/Enums/CompanyVerificationStatus.cs",
    "$A/UseCases/Company",
    "$A/Interfaces/ICompanyRepository.cs",
    "$I/Persistence/Configurations/CompanyConfiguration.cs",
    "$I/Persistence/Repositories/CompanyRepository.cs",
    "$W/Controllers/CompaniesController.cs",
    "$W/Models/RegisterCompanyRequest.cs",
    "$W/Models/RejectCompanyRequest.cs"
)

Write-Host "Moving CV service files..."
Move-ToService "CV" @(
    "$D/Entities/CV.cs",
    "$D/Enums/CVSource.cs",
    "$D/Enums/CVStatus.cs",
    "$A/UseCases/CV",
    "$A/Interfaces/ICVRepository.cs",
    "$A/Interfaces/ICVTextExtractor.cs",
    "$A/Interfaces/IEmbeddingService.cs",
    "$I/Persistence/Configurations/CVConfiguration.cs",
    "$I/Persistence/Repositories/CVRepository.cs",
    "$I/AI/FakeCVTextExtractor.cs",
    "$I/AI/FakeEmbeddingService.cs",
    "$W/Controllers/CVsController.cs",
    "$W/Models/UploadCVRequest.cs",
    "$W/Models/UpdateCVContentRequest.cs"
)

Write-Host "Moving Recruitment service files..."
Move-ToService "Recruitment" @(
    "$D/Entities/JobDescription.cs",
    "$D/Entities/Interview.cs",
    "$D/Enums/JobDescriptionStatus.cs",
    "$D/Enums/InterviewStatus.cs",
    "$A/UseCases/JobDescription",
    "$A/UseCases/Interview",
    "$A/UseCases/Matching",
    "$A/Common/CosineSimilarityCalculator.cs",
    "$A/DTOs/CandidateMatchResult.cs",
    "$A/Interfaces/IJobDescriptionRepository.cs",
    "$A/Interfaces/IInterviewRepository.cs",
    "$I/Persistence/Configurations/JobDescriptionConfiguration.cs",
    "$I/Persistence/Configurations/InterviewConfiguration.cs",
    "$I/Persistence/Repositories/JobDescriptionRepository.cs",
    "$I/Persistence/Repositories/InterviewRepository.cs",
    "$W/Controllers/JobDescriptionsController.cs",
    "$W/Controllers/InterviewsController.cs",
    "$W/Controllers/MatchingController.cs",
    "$W/Models/CancelInterviewRequest.cs",
    "$W/Models/RecordInterviewResultRequest.cs",
    "$W/Models/RejectJobDescriptionRequest.cs",
    "$W/Models/ScheduleInterviewRequest.cs",
    "$W/Models/SubmitJobDescriptionRequest.cs"
)

Write-Host "Moving schema owner (DbContext + migrations) to src/Database..."
Move-Git "$I/Persistence/SmartRecruitmentDbContext.cs" "src/Database/SmartRecruitmentDbContext.cs"
Move-Git "$I/Migrations" "src/Database/Migrations"

# ---------------------------------------------------------------- 3. Drop in the new files (overwrites the 5 Recruitment handlers that changed)
Write-Host "Adding new files (gateway, Program.cs, DbContexts, internal endpoints, HTTP gateways)..."
Expand-Archive -Path "split-new-files.zip" -DestinationPath "." -Force

# ---------------------------------------------------------------- 4. Point each repository at its own DbContext
Set-DbContextName "Candidate"   "CandidateDbContext"
Set-DbContextName "Company"     "CompanyDbContext"
Set-DbContextName "CV"          "CVDbContext"
Set-DbContextName "Recruitment" "RecruitmentDbContext"

# ---------------------------------------------------------------- 5. Solution file: swap old projects for the new ones
Write-Host "Updating SmartRecruitment.sln..."
dotnet sln SmartRecruitment.sln remove `
    "$W/SmartRecruitment.API.csproj" `
    "$A/SmartRecruitment.Application.csproj" `
    "$D/SmartRecruitment.Domain.csproj" `
    "$I/SmartRecruitment.Infrastructure.csproj"
if ($LASTEXITCODE -ne 0) { throw "dotnet sln remove failed" }

dotnet sln SmartRecruitment.sln add `
    "src/ApiGateway/ApiGateway.csproj" `
    "src/Database/Database.csproj" `
    "src/Services/Candidate/Candidate.Service.csproj" `
    "src/Services/Company/Company.Service.csproj" `
    "src/Services/CV/CV.Service.csproj" `
    "src/Services/Recruitment/Recruitment.Service.csproj"
if ($LASTEXITCODE -ne 0) { throw "dotnet sln add failed" }

# ---------------------------------------------------------------- 6. Remove what is left of the old projects (Program.cs, csproj, bin/obj...)
Write-Host "Removing leftovers of the old monolith projects..."
foreach ($old in @($W, $A, $D, $I)) {
    git rm -r -q --ignore-unmatch $old
    if (Test-Path $old) { Remove-Item $old -Recurse -Force }
}

# ---------------------------------------------------------------- 7. Stage and build
git add src SmartRecruitment.sln
Write-Host "Building..."
dotnet build SmartRecruitment.sln
if ($LASTEXITCODE -ne 0) { Write-Warning "Build failed - copy the errors and send them over. Nothing has been committed." }
else { Write-Host "Build OK. Review with 'git status', then commit." }
