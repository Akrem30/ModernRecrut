using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using ModernRecrut.Postulations.API.Services;

namespace ModernRecrut.Postulations.API.UnitTests.Services
{
	public class GenerationEvaluationServiceTests
	{
		
		[Theory]
		[InlineData(8000, "ApplicationPostulation", "Salaire inférieur à la norme")]
		[InlineData(31000, "ApplicationPostulation", "Salaire proche mais inférieur à la norme")]
		[InlineData(61000, "ApplicationPostulation", "Salaire dans la norme")]
		[InlineData(89000, "ApplicationPostulation", "Salaire proche mais supérieur à la norme")]
		[InlineData(120000, "ApplicationPostulation", "Salaire supérieur à la norme")]
		public void GenererEvaluation_PretentionSalariale_Retourne_Note(decimal pretentionSalariale, string nomEmetteur,string description)
		{
			//Etant donné
			GenerationEvaluationService service = new GenerationEvaluationService();

			//Lorsque
			var note = service.GenererEvaluation(pretentionSalariale);

			//Alors
			note.NomEmetteur.Should().Be(nomEmetteur);
			note.Description.Should().Be(description);
		}
	}
}
