using ModernRecrut.Postulations.API.Entities;
using ModernRecrut.Postulations.API.Interfaces;

namespace ModernRecrut.Postulations.API.Services
{
	public class GenerationEvaluationService : IGenerationEvaluationService
	{

		public Note GenererEvaluation(decimal pretentionSalariale)
		{
			string description = "";


			switch (pretentionSalariale)
			{

				case decimal p when p < 20000:
					description = "Salaire inférieur à la norme";
					break;

				case decimal p when p >= 20000 && p <= 39999:
					description = "Salaire proche mais inférieur à la norme";
					break;


				case decimal p when p >= 40000 && p <= 79999:
					description = "Salaire dans la norme";
					break;

				case decimal p when p >= 80000 && p <= 99999:
					description = "Salaire proche mais supérieur à la norme";
					break;


				case decimal p when p >= 100000:
					description = "Salaire supérieur à la norme";
					break;
			}


			return new Note { NomEmetteur = "ApplicationPostulation", Description = description };
		}





	}

}






