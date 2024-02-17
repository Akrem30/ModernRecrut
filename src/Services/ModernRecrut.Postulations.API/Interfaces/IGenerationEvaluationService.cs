using ModernRecrut.Postulations.API.Entities;

namespace ModernRecrut.Postulations.API.Interfaces
{
    public interface IGenerationEvaluationService
    {
        public Note GenererEvaluation(decimal pretentionSalariale);
    }
}
