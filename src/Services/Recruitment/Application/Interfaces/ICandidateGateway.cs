namespace SmartRecruitment.Application.Interfaces
{
    // Port to the Candidate Service. The Recruitment Service does not own Candidate data,
    // so every status change on a candidate goes through that service's API.
    // Implementations throw KeyNotFoundException (candidate missing) or InvalidOperationException (transition not allowed).
    public interface ICandidateGateway
    {
        Task EnterInterviewAsync(Guid candidateId);
        Task MarkAsEmployedAsync(Guid candidateId);
        Task ReturnToUnemployedAsync(Guid candidateId);
    }
}
