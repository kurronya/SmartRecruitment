using SmartRecruitment.Application.Interfaces;

namespace SmartRecruitment.Application.UseCases.CV.UpdateCVContent
{
    public class UpdateCVContentHandler
    {
        private readonly ICVRepository _cvRepository;

        public UpdateCVContentHandler(ICVRepository cvRepository)
        {
            this._cvRepository = cvRepository;
        }

        public async Task HandleAsync(UpdateCVContentCommand command)
        {
            var cv = await this._cvRepository.GetByIdAsync(command.CVId);
            if (cv == null)
                throw new KeyNotFoundException("CV not found.");

            // Entity enforces that only a draft CV can be edited
            cv.UpdateContent(command.NewContent);

            await this._cvRepository.UpdateAsync(cv);
        }
    }
}