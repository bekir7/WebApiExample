using Entities.LinkModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
	[ApiController]
	[Route("api")]
	[ApiExplorerSettings(GroupName = "v1")]
	public class RootController:ControllerBase
	{
		private readonly LinkGenerator linkGenerator;

		public RootController(LinkGenerator linkGenerator)
		{
			this.linkGenerator = linkGenerator;
		}
		[HttpGet(Name="GetRoot")]
		public async Task<IActionResult> GetRoot([FromHeader(Name ="Accept")]string mediaType)
		{
			if (mediaType.Contains("application/vnd.anan.apiroot"))
			{
				var list=new List<Link>()
				{
					new Link()
					{
						Href=linkGenerator.GetUriByName(HttpContext,nameof(GetRoot),new {}),
						Rel="self",
						Method="GET",
					},
					new Link()
					{
						Href=linkGenerator.GetUriByName(HttpContext,nameof(BooksController.GetAllBooksAsync),new {}),
						Rel="self",
						Method="GET",
					},
					new Link()
					{
						Href=linkGenerator.GetUriByName(HttpContext,nameof(BooksController.CreateOneBookAsync),new {}),
						Rel="self",
						Method="POST",
					},
				};

				return Ok(list);
			}
			return NoContent();
		}
	}
}
