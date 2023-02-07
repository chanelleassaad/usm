namespace usm.Exceptions;

public class NotATeacherException : Exception
{
    public NotATeacherException(string message)
        :base(message){}
}