using CSharpFunctionalExtensions;

using Logic.Dtos;
using Logic.Students;
using Logic.Utils;

using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;

namespace Api.Controllers
{
    [Route("api/students")]
    public sealed class StudentController : BaseController
    {
        private readonly Messages _messages;

        public StudentController( Messages messages)
        {
            _messages = messages;
        }

        [HttpGet]
        public IActionResult GetList(string enrolled, int? number)
        {
            List<StudentDto> list = _messages.Dispatch(new GetListQuery(enrolled, number));
            return Ok(list);
        }

        [HttpPost]
        public IActionResult Register([FromBody] NewStudentDto dto)
        {
            var command = new RegisterCommand(
                dto.Name, dto.Email,
                dto.Course1, dto.Course1Grade,
                dto.Course2, dto.Course2Grade);

            Result result = _messages.Dispatch(command);
            return result.IsSuccess ? Ok() : Error(result.Error);
        }

        [HttpDelete("{id}")]
        public IActionResult Unregister(long id)
        {
            Result result = _messages.Dispatch(new UnregisterCommand(id));
            return result.IsSuccess ? Ok() : Error(result.Error);
        }

        [HttpPost("{id}/enrollments")]
        public IActionResult Enroll(long id, [FromBody] StudentEnrollmentDto dto)
        {
            Result result = _messages.Dispatch(new EnrollCommand(id, dto.Course, dto.Grade));
            return result.IsSuccess ? Ok() : Error(result.Error);
        }

        [HttpPut("{id}/enrollments/{enrollmentNumber}")]
        public IActionResult Transfer(long id, int enrollmentNumber, [FromBody] StudentTransferDto dto)
        {
            Result result = _messages.Dispatch(new TransferCommand(id, enrollmentNumber, dto.Course, dto.Grade));
            return result.IsSuccess ? Ok() : Error(result.Error);
        }

        [HttpPost("{id}/enrollments/{enrollmentNumber}/deletion")]
        public IActionResult Disenroll(long id, int enrollmentNumber, [FromBody] StudentDisenrollmentDto dto)
        {
            Result result = _messages.Dispatch(new DisenrollCommand(id, enrollmentNumber, dto.Comment));
            return result.IsSuccess ? Ok() : Error(result.Error);
        }

        [HttpPut("{id}")]
        public IActionResult EditPersonalInfo(long id, [FromBody] StudentPersonalInfoDto dto)
        {
            var command = new EditPersonalInfoCommand(id, dto.Email, dto.Name);
            Result result = _messages.Dispatch(command);

            return result.IsSuccess ? Ok() : Error(result.Error);
        }
    }
}
