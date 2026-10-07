using LibraryManagementSystem.API.Dtos;
using LibraryManagementSystem.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/libraries")]
    public class LibrariesController : ControllerBase
    {
        private readonly ILibraryService _libraryService;

        public LibrariesController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var libraries = await _libraryService.GetAllAsync();
            return Ok(libraries);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _libraryService.GetByIdAsync(id);

            if (!result.Success)
                return StatusCode(result.StatusCode, new { status = result.StatusCode, message = result.Message });

            return StatusCode(result.StatusCode, new { status = result.StatusCode, data = result.Data });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateLibraryDto dto)
        {
            var result = await _libraryService.CreateAsync(dto);

            if (!result.Success)
                return StatusCode(result.StatusCode, new { status = result.StatusCode, message = result.Message });

            return StatusCode(result.StatusCode, new { status = result.StatusCode, message = result.Message, data = result.Data });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateLibraryDto dto)
        {
            var result = await _libraryService.UpdateAsync(id, dto);
            return StatusCode(result.StatusCode, new { status = result.StatusCode, message = result.Message });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _libraryService.DeleteAsync(id);
            return StatusCode(result.StatusCode, new { status = result.StatusCode, message = result.Message });
        }
    }
}
