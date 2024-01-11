﻿using Entities.DataTransferObject;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
	//[ApiVersion("1.0")]
	[ServiceFilter(typeof(LogFilterAttribute))]
	[ApiController]
	[Route("api/books")]
	[ApiExplorerSettings(GroupName = "v1")]
	//[ResponseCache(CacheProfileName="5mins")]
	//[HttpCacheExpiration(CacheLocation=CacheLocation.Public,MaxAge =80)]
	public class BooksController : ControllerBase
	{
		private readonly IServiceManager _manager;

		public BooksController(IServiceManager manager)
		{
			_manager = manager;
		}
		[Authorize(Roles = "User,Editor,Admin")]
		[HttpHead]
		[HttpGet(Name = "GetAllBooksAsync")]
		[ServiceFilter(typeof(ValidateMediaTypeAttribute))]
		//[ResponseCache(Duration =60)]
		public async Task<IActionResult> GetAllBooksAsync([FromQuery]BookParameters bookParameters)
		{
			var linkParameters = new LinkParameters()
			{
				BookParameters = bookParameters,
				HttpContext = HttpContext
			};
			var result = await _manager
				.BookService
				.GetAllBooksAsync(linkParameters, false);

			
			Response.Headers.Add("X-Pagination", 
				JsonSerializer.Serialize(result.metaData));


			return result.linkResponse.HasLinks ?
				Ok(result.linkResponse.LinkedEntities) :
				Ok(result.linkResponse.ShapedEntities);


		}

		[Authorize]
		[HttpGet("details")]
		public async Task<IActionResult> GetAllBooksWithDetailsAsync()
		{
			return Ok(await _manager
				.BookService
				.GetAllBooksWithDetailsAsync(false));
		}



		[Authorize]
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetBooksByIdAsync(int id)
		{
			
				var book =await _manager.BookService.GetOneBookByIdAsync(id, false);
				return Ok(book);
			

		}

		[Authorize(Roles = "Editor,Admin")]
		[ServiceFilter(typeof(ValidationAttributeFilter))]
		[HttpPost(Name = "CreateOneBookAsync")]
		public async Task<IActionResult> CreateOneBookAsync([FromBody] BookDtoForInsertion bookDto)
		{
			var book=await _manager.BookService.CreateOneBookAsync(bookDto);
            return StatusCode(201, book);
		}

		[Authorize(Roles = "Editor,Admin")]
		[ServiceFilter(typeof(ValidationAttributeFilter))]
		[HttpPut("{id:int}")]
		public async  Task<IActionResult> UpdateBookAsync(int id, [FromBody] BookDtoForUpdate bookDto)
		{
				await _manager.BookService.UpdateOneBookAsync(id, bookDto, false);
				return NoContent();
		}

		[Authorize(Roles = "Admin")]
		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeleteOneBookAsync(int id)
		{
			
				await _manager.BookService.DeleteOneBookAsync(id, false);
				return NoContent();
			
			
		}

		[Authorize(Roles = "Editor,Admin")]
		[HttpPatch("{id:int}")]
		public async Task<IActionResult> PartiallyUpdateBookAsync(int id, JsonPatchDocument<BookDtoForUpdate> bookPatch)
		{
			if(bookPatch == null)
				return BadRequest();

			var result =await _manager.BookService.GetOneBookForPatchAsync(id, false);	

			bookPatch.ApplyTo(result.bookDtoForUpdate,ModelState);

			TryValidateModel(result.bookDtoForUpdate);

			if(!ModelState.IsValid)
				return UnprocessableEntity(ModelState);

			await _manager.BookService.SaveChangesForPatchAsync(result.bookDtoForUpdate, result.book);
				return NoContent();
			
			
		}

		[Authorize]
		[HttpOptions]
		public IActionResult GetBookOptions()
		{
			Response.Headers.Add("Allow", "GET,PUT,POST,PATCH,DELETE,HEAD,OPTIONS");

			return Ok();
		}
	}
}
