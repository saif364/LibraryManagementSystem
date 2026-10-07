using LibraryManagementSystem.API.Dtos;
using LibraryManagementSystem.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBookService bookService, ILogger<BooksController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("getting all");

            var books = await _bookService.GetAllAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("getting singe book= {BookId}", id);

            var result = await _bookService.GetByIdAsync(id);

            if (!result.Success)
            {
                _logger.LogWarning("book {BookId} not found", id);
                return StatusCode(result.StatusCode, new { status = result.StatusCode, message = result.Message });
            }

            return StatusCode(result.StatusCode, new { status = result.StatusCode, data = result.Data });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookDto dto)
        {
            _logger.LogInformation("creating book");

            var result = await _bookService.CreateAsync(dto);

            if (!result.Success)
            {
                _logger.LogWarning("failed creating book with ISBN {Isbn}: {Message}", dto.ISBN, result.Message);
                return StatusCode(result.StatusCode, new { status = result.StatusCode, message = result.Message });
            }

            _logger.LogInformation("book created with id {BookId}", result.Data?.Id);
            return StatusCode(result.StatusCode, new { status = result.StatusCode, message = result.Message, data = result.Data });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateBookDto dto)
        {
            _logger.LogInformation("updating book with id {BookId}", id);

            var result = await _bookService.UpdateAsync(id, dto);

            if (!result.Success)
                _logger.LogWarning("fald to update book with id {BookId}: {Message}", id, result.Message);

            return StatusCode(result.StatusCode, new { status = result.StatusCode, message = result.Message });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("deleting book with id {BookId}", id);

            var result = await _bookService.DeleteAsync(id);

            if (!result.Success)
                _logger.LogWarning("failed to delete book with id {BookId}: {Message}", id, result.Message);

            return StatusCode(result.StatusCode, new { status = result.StatusCode, message = result.Message });
        }
    }
}
