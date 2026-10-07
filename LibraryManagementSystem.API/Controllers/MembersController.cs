using LibraryManagementSystem.API.Dtos;
using LibraryManagementSystem.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/members")]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var members = await _memberService.GetAllAsync();
            return Ok(members);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _memberService.GetByIdAsync(id);

            if (!result.Success)
                return StatusCode(result.StatusCode, new { status = result.StatusCode, message = result.Message });

            return StatusCode(result.StatusCode, new { status = result.StatusCode, data = result.Data });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberDto dto)
        {
            var result = await _memberService.CreateAsync(dto);

            if (!result.Success)
                return StatusCode(result.StatusCode, new { status = result.StatusCode, message = result.Message });

            return StatusCode(result.StatusCode, new { status = result.StatusCode, message = result.Message, data = result.Data });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateMemberDto dto)
        {
            var result = await _memberService.UpdateAsync(id, dto);
            return StatusCode(result.StatusCode, new { status = result.StatusCode, message = result.Message });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _memberService.DeleteAsync(id);
            return StatusCode(result.StatusCode, new { status = result.StatusCode, message = result.Message });
        }
    }
}
