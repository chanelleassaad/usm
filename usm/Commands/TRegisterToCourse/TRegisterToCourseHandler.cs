using MediatR;
using usm.Exceptions;
using usm.Models;

namespace usm.Commands.TRegisterToCourse;

public class TRegisterToCourseHandler: IRequestHandler<TRegisterToCourseRequest, TRegisterToCourseResponse>
{
    private readonly PostgresContext _context;

    public TRegisterToCourseHandler(PostgresContext context)
    {
        _context = context;
    }
    
    public async Task<TRegisterToCourseResponse> Handle(TRegisterToCourseRequest request, CancellationToken cancellationToken)
    { 
        var roleId = _context.Users.ToList().FirstOrDefault(t => t.Id == request.TeacherId)!.RoleId;
        var teacherRoleId = _context.Roles.ToList()
            .FirstOrDefault(r => r.Name.Equals("teacher", StringComparison.OrdinalIgnoreCase))!.Id;
        if (roleId != teacherRoleId)
        {
            throw new NotATeacherException("You are not a teacher");
        }
        
        var courseId = _context.Courses.ToList().FirstOrDefault(c => c.Name.Equals(request.CourseName, StringComparison.OrdinalIgnoreCase))!.Id;
        if (_context.TeacherPerCourses.ToList().FirstOrDefault(t => t.CourseId == courseId) != null)
        {
            throw new CourseAlreadyRegisteredException("Course is already registered by another teacher");
        }
            
        var newRegister = new TeacherPerCourse
        {
            Id = default,
            TeacherId = request.TeacherId,
            CourseId = courseId,
        };

        _context.TeacherPerCourses.Add(newRegister);
        await _context.SaveChangesAsync(cancellationToken);
        
        return new TRegisterToCourseResponse()
        {
            done = "done"
        };
    }
}
