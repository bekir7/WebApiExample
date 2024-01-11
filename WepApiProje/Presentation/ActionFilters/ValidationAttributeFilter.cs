using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ActionFilters
{
	public class ValidationAttributeFilter : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var controller = context.RouteData.Values["controller"];
			var action = context.RouteData.Values["action"];

			//Dto
			var param = context.ActionArguments
				.SingleOrDefault(p => p.Value.ToString().Contains("Dto")).Value;

			if (param == null)
			{
				context.Result = new BadRequestObjectResult($"Dto is null" + 
					$"Controller: {controller}" +
					$"Action:{action}");
				return;
			}
			else
			{
				if(!context.ModelState.IsValid)
				   context.Result=new UnprocessableEntityObjectResult(context.ModelState);
				
			}
		}
	}
}
