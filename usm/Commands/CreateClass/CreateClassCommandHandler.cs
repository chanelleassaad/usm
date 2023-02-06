using MediatR;
using NpgsqlTypes;
using usm.Models;

namespace usm.Commands.CreateClass;

public class CreateClassCommandHandler: IRequestHandler<CreateClassCommand, CreateClassCommandResponse>
{
    private readonly PostgresContext _context;

    public CreateClassCommandHandler(PostgresContext context)
    {
        _context = context;
    }
    
    public async Task<CreateClassCommandResponse> Handle(CreateClassCommand request, CancellationToken cancellationToken)
    {
        Course course = new Course();
        course.Name = request.CourseName;
        course.MaxStudentsNumber = request.CourseCapacity;
        course.EnrolmentDateRange = new NpgsqlRange<DateOnly>(new DateOnly(2023, 2, 6), new DateOnly(2023, 2, 10));
        
        _context.Courses.Add(course);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateClassCommandResponse()
        {
            Course = course
        };
    }
}