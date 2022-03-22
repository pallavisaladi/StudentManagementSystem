namespace studentmanagementsystem
{
    using System;

    public class InvalidStudentException : ApplicationException
    {
        public InvalidStudentException()
        {
        }
        public InvalidStudentException(string? message) : base(message)
        {
        }
    }
}