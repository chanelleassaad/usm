using System;
using System.Collections.Generic;

namespace usm.Models;

public partial class TeacherPerCourse
{
    public long Id { get; set; }

    public long TeacherId { get; set; }

    public long CourseId { get; set; }

    public virtual ICollection<ClassEnrollment> ClassEnrollments { get; } = new List<ClassEnrollment>();

    public virtual Course Course { get; set; } = null!;

    public virtual User Teacher { get; set; } = null!;

    public virtual ICollection<TeacherPerCoursePerSessionTime> TeacherPerCoursePerSessionTimes { get; } = new List<TeacherPerCoursePerSessionTime>();
}
