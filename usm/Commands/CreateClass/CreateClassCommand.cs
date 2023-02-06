using MediatR;

namespace usm.Commands.CreateClass;

public class CreateClassCommand: IRequest<CreateClassCommandResponse>
{
    public string CourseName { get; set; }
    public int CourseCapacity { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }

}