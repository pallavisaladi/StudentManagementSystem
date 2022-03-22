namespace studentmanagementsystem
{
    using System;

    public class Repeatedexception: ApplicationException
    {
        public Repeatedexception()
        {
        }

        public Repeatedexception(string? message) : base(message)
        {
        }
    }
}