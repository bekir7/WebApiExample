using AutoMapper;
using Entities.DataTransferObject;
using Entities.Exceptions;
using Entities.LinkModels;
using Entities.Models;
using Entities.RequestFeatures;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Entities.Exceptions.BadRequestException;

namespace Services
{
	public class BookManager : IBookService
	{
		private readonly ICategoryService _categoryService;
		private readonly IRepositoryManager manager;
		private readonly ILoggerService logger;
		private readonly IMapper mapper;
		private readonly IBookLinks bookLinks;

		public BookManager(IRepositoryManager manager, ILoggerService logger, IMapper mapper, IBookLinks bookLinks, ICategoryService categoryService)
		{
			this.manager = manager;
			this.logger = logger;
			this.mapper = mapper;
			this.bookLinks = bookLinks;
			_categoryService = categoryService;
		}

		public async Task<BookDto> CreateOneBookAsync(BookDtoForInsertion book)
		{
			var category = await _categoryService.GetOneCategoryByIdAsync(book.CategoryId, false);

			var entity=mapper.Map<Book>(book);
			entity.CategoryId = book.CategoryId;
			manager.Book.CreateOneBook(entity);
			await manager.SaveAsync();
			return mapper.Map<BookDto>(entity);
		}

		public async Task DeleteOneBookAsync(int id, bool trackChanges)
		{
			var entity =await GetOneBookByIdAndCheckExists(id, trackChanges);
			manager.Book.DeleteOneBook(entity);
			await manager.SaveAsync();
		}

		public async Task<(LinkResponse linkResponse, MetaData metaData)> GetAllBooksAsync(LinkParameters linkParameters, bool trackChanges)
		{

			if (!linkParameters.BookParameters.ValidPriceRange)
				throw new PriceOutofRangeBadRequestException();
			
			var booksWithMetaData=await manager
				.Book
				.GetAllBooksAsync(linkParameters.BookParameters, trackChanges);
			
			var booksDto= mapper.Map<IEnumerable<BookDto>>(booksWithMetaData);

			var links = bookLinks.TryGenerateLinks(booksDto, linkParameters.BookParameters.Fields, linkParameters.HttpContext);
			return (linkResponse:links,metaData: booksWithMetaData.MetaData);
		}

		public async Task<List<Book>> GetAllBooksAsync(bool trackChanges)
		{
			var books=await manager.Book.GetAllBooksAsync(trackChanges);
			return books;
		}

		public async Task<IEnumerable<Book>> GetAllBooksWithDetailsAsync(bool trackChanges)
		{
			return await manager.Book.GetAllBooksWithDetailsAsync(trackChanges);
		}

		public async Task<BookDto> GetOneBookByIdAsync(int id, bool trackChanges)
		{
			var book = await GetOneBookByIdAndCheckExists(id, trackChanges);
			return mapper.Map<BookDto>(book);
		}

		public async Task<(BookDtoForUpdate bookDtoForUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges)
		{
			var book = await GetOneBookByIdAndCheckExists(id, trackChanges);
			var bookDtoForUpdate=mapper.Map<BookDtoForUpdate>(book);
			return (bookDtoForUpdate, book);
		}

		public async Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book)
		{
			mapper.Map(bookDtoForUpdate,book);
			await manager.SaveAsync();
		}

		public async Task UpdateOneBookAsync(int id, BookDtoForUpdate bookDto, bool trackChanges)
		{
			//check entity
			var entity = await GetOneBookByIdAndCheckExists(id, trackChanges);
			entity = mapper.Map<Book>(bookDto);
			manager.Book.Update(entity);
			await manager.SaveAsync();
		}
		
		private async Task<Book> GetOneBookByIdAndCheckExists(int id,bool trackChanges)
		{
			//check entity
			var entity = await manager.Book.GetOneBookByIdAsync(id, trackChanges);
			if (entity == null)
				throw new BookNotFoundException(id);
			return entity;
		}
	}
}
