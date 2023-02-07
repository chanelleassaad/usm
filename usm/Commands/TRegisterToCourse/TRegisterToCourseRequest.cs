using MediatR;

namespace usm.Commands.TRegisterToCourse;

public class TRegisterToCourseRequest: IRequest<TRegisterToCourseResponse>
{
    public int TeacherId { get; set; }
    public string CourseName { get; set; }
}