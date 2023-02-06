using MediatR;
using Microsoft.AspNetCore.Mvc;
using NpgsqlTypes;
using usm.Commands.CreateClass;
using usm.Models;

namespace usm.Controllers;

[ApiController]
[Route("api/class")]
public class ClassController: ControllerBase
{
    private readonly IMediator _mediator;
    public ClassController(IMediator mediator) {
        _mediator = mediator;
    }
    [HttpPost("createCourse")]
    public async Task<CreateClassCommandResponse> CreateCourse([FromQuery] string name, [FromQuery] int maximumNumberOfAllowedStudent, [FromBody] NpgsqlRange<DateTime> date)
    {
        CreateClassCommand s = new CreateClassCommand();
        s.CourseName = name;
        s.CourseCapacity = maximumNumberOfAllowedStudent;
        s.DateStart = date.LowerBound;
        s.DateEnd = date.UpperBound;
        return await _mediator.Send(s)!;
    }
}