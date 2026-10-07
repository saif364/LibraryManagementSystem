using LibraryManagementSystem.API.Dtos;
using LibraryManagementSystem.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/borrowrecords")]
    public class BorrowRecordsController : ControllerBase
    {
        private readonly IBorrowRecordService _borrowRecordService;

        public BorrowRecordsController(IBorrowRecordService borrowRecordService)
        {
            _borrowRecordService = borrowRecordService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await _borrowRecordService.GetAllAsync();
            return Ok(records);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _borrowRecordService.GetByIdAsync(id);

            if (!result.Success)
                return StatusCode(result.StatusCode, new { status = result.StatusCode, message = result.Message });

            return StatusCode(result.StatusCode, new { status = result.StatusCode, data = result.Data });
        }

        [HttpPost("borrow")]
        public async Task<IActionResult> Borrow(BorrowRequestDto dto)
        {
            var result = await _borrowRecordService.BorrowAsync(dto);

            if (!result.Success)
                return StatusCode(result.StatusCode, new { status = result.StatusCode, message = result.Message });

            return StatusCode(result.StatusCode, new { status = result.StatusCode, message = result.Message, data = result.Data });
        }

        [HttpPut("{id}/return")]
        public async Task<IActionResult> Return(int id)
        {
            var result = await _borrowRecordService.ReturnAsync(id);
            return StatusCode(result.StatusCode, new { status = result.StatusCode, message = result.Message });
        }
    }
}
