namespace studentmanagementsystem
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;

    class InMemoryAppEngine : AppEngine
    {
        info info = new info();
        Course c;
        public List<Student> studentslist = new List<Student>();
        public List<Course> courseslist = new List<Course>();
        public List<Enroll> enrolllist = new List<Enroll>();

        public static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
        public void introduce(Course course)
        {
            courseslist.Add(course);
            int rows = 0;
            if (course.Id != 0 && course.Name != " " && course.Duration != 0 && course.Fees != 0)
            {
                SqlCommand cmd = new SqlCommand("spInsertCourse", con);
                try
                {
                    con.Open();
                    Console.WriteLine("Connected successfully");
                    if (course.GetType() == typeof(DegreeCourse))
                    {
                        DegreeCourse degreec = (DegreeCourse)course;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Cid", degreec.Id);
                        cmd.Parameters.AddWithValue("@Cname", degreec.Name);
                        cmd.Parameters.AddWithValue("@Cduration", degreec.Duration);
                        cmd.Parameters.AddWithValue("@Cfees", degreec.Fees);
                        cmd.Parameters.AddWithValue("@Ccourse", "Degree");
                        cmd.Parameters.AddWithValue("@clevel", degreec.level.ToString());
                        cmd.Parameters.AddWithValue("@Cplacement", degreec.Isplacementavailable.ToString());
                        cmd.Parameters.AddWithValue("@Ctype", "NA");
                        cmd.Parameters.AddWithValue("@Monthlyfee", degreec.calculateMonthlyFee().ToString());
                        try
                        {
                            rows = cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine(rows + " rows added!");
                        Console.WriteLine("Course Id " + course.Id + " with course name " + course.Name + "has successfully added");
                        con.Close();
                    }
                    else if (course.GetType() == typeof(DiplomaCourse))
                    {
                        DiplomaCourse diplomac = (DiplomaCourse)course;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Cid", diplomac.Id);
                        cmd.Parameters.AddWithValue("@Cname", diplomac.Name);
                        cmd.Parameters.AddWithValue("@Cduration", diplomac.Duration);
                        cmd.Parameters.AddWithValue("@Cfees", diplomac.Fees);
                        cmd.Parameters.AddWithValue("@Ccourse", "Diploma");
                        cmd.Parameters.AddWithValue("@clevel", "NA");
                        cmd.Parameters.AddWithValue("@Cplacement", "NA");
                        cmd.Parameters.AddWithValue("@Ctype", diplomac.type.ToString());
                        cmd.Parameters.AddWithValue("@Monthlyfee", diplomac.calculateMonthlyFee().ToString());
                        try
                        {
                            rows = cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine(rows + " rows added!");
                        Console.WriteLine("Course Id " + course.Id + " with course name " + course.Name + " has successfully added");
                        con.Close();
                    }
                    else
                    {
                        Console.WriteLine("Invalid course name");
                    }
                }
                 catch (SqlException e)
                {
                    Console.WriteLine("cannot connect to the database");
                }
            }
            else
            {
                Console.WriteLine("Please enter the values for the course details to be added to the database");
            }
        }
        public void register(Student student)
        {
            try
            {
                con.Open();
                Console.WriteLine("Connected successfully");
                SqlCommand cmd = new SqlCommand();
                int rows = 0;
                cmd = new SqlCommand("spInsertStudent", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@studentid", student.Id);
                cmd.Parameters.AddWithValue("@studentname", student.Name);
                cmd.Parameters.AddWithValue("@dob", student.Dob);
                try
                {
                    rows = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.WriteLine(rows + " rows added!");
                Console.WriteLine("student Id " + student.Id + " has successfully added");
                con.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            studentslist.Add(student);
        }
        public List<Student> listOfStudents()
        {
            //Console.WriteLine("Displaying from the list");
            //if (studentslist.Count() > 0)
            //{
            //    foreach (var student in studentslist)
            //    {
            //        info.display(student);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("No student details to display from the list");
            //}
            try
            {
                con.Open();
                Console.WriteLine("Connected successfully");
                Console.WriteLine("Displaying students from the database");
                SqlCommand cmd = new SqlCommand("select * from student_table", con);
                SqlDataReader dr = cmd.ExecuteReader();
                Console.WriteLine("Records inserted into the database are: ");
                if (dr.HasRows)
                {
                    Console.WriteLine("{0}\t{1}\t{2}", dr.GetName(0), dr.GetName(1), dr.GetName(2));
                    while (dr.Read())
                    {
                        Console.WriteLine(dr.GetInt32(0) + "\t\t" + dr.GetString(1) + "\t\t" + dr.GetDateTime(2));
                    }
                }
                else
                {
                    Console.WriteLine("No students to display from the database");
                }
                dr.Close();
                con.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }
        public List<Course> listOfCourses(string choice)
        {
            //Console.WriteLine("Displaying from the list");
            //if (courseslist.Count() > 0)
            //{
            //    foreach (var course in courseslist)
            //    {
            //        info.display(course);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("No courses to display from the list");
            //}
            try
            {
                con.Open();
                Console.WriteLine("Connected successfully");
                Console.WriteLine("Displaying courses from the database");
                SqlCommand cmd = new SqlCommand("Select * from course where Ccourse=@Ccourse", con);
                cmd.CommandType = CommandType.Text;
                SqlParameter param = new SqlParameter("@Ccourse", choice);
                cmd.Parameters.Add(param);
                SqlDataReader dr = cmd.ExecuteReader();
                try
                {
                    if (dr.HasRows)
                    {
                        Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}", dr.GetName(0), dr.GetName(1), dr.GetName(2), dr.GetName(3), dr.GetName(4), dr.GetName(5), dr.GetName(6), dr.GetName(7), dr.GetName(8));
                        while (dr.Read())
                        {
                            Console.WriteLine(dr[0] + "\t" + dr[1] + "\t" + dr[2] + "\t" + dr[3] + "\t" + dr[4] + "\t" + dr[5] + "\t" + dr[6] + "\t" + dr[7] + "\t" + dr[8] + "\t");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No courses to display from the database");
                    }
                    con.Close();
                    dr.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public void addEnrollment(Student student, Course course)
        {
            try
            {
                con.Open();
                Console.WriteLine("Connected successfully");
                SqlCommand cmd = new SqlCommand();
                int rows = 0;
                int repeated;
                Console.WriteLine("Enter student Id:");
                int id = Convert.ToInt32(Console.ReadLine());
                try
                {
                    cmd = new SqlCommand("select @studentid from student_table where " + id + " = @studentid", con);
                    cmd.Parameters.AddWithValue("@studentid", id);
                    repeated = cmd.ExecuteNonQuery();
                    if (repeated > 0)
                    {
                        Console.WriteLine("Enter your choice\n1.Degree \n2.Diploma");
                        string choice = Console.ReadLine();
                        con.Close();
                        listOfCourses(choice);
                        Console.WriteLine("Enter Course Id:");
                        int courseid = Convert.ToInt32(Console.ReadLine());
                        con.Open();
                        cmd = new SqlCommand("select @Cid from course where " + courseid + " = @Cid AND '" + choice + "' = @Ccourse", con);
                        DateTime enrollmentdate = DateTime.UtcNow;
                        int count;
                        int idcount;
                        try
                        {
                            cmd = new SqlCommand("select count(*) from enroll where " + id + " = @studentid AND " + courseid + " = @Cid", con);
                            cmd.Parameters.AddWithValue("@studentid", id);
                            cmd.Parameters.AddWithValue("@Cid", courseid);
                            cmd.CommandType = CommandType.Text;
                            idcount = cmd.ExecuteNonQuery();
                            if (idcount > 0)
                            {
                                try
                                {
                                    cmd = new SqlCommand("select count(@studentid) from enroll where " + id + " =  @studentid", con);
                                    cmd.Parameters.AddWithValue("@studentid", id);
                                    cmd.CommandType = CommandType.Text;
                                    count = (int)cmd.ExecuteScalar();
                                    if (count <= 4)
                                    {
                                        cmd = new SqlCommand("spInsertenroll", con);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@studentid", id);
                                        cmd.Parameters.AddWithValue("@courseid", courseid);
                                        cmd.Parameters.AddWithValue("@enrollmentdate", enrollmentdate);
                                        try
                                        {
                                            rows = cmd.ExecuteNonQuery();
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                        Console.WriteLine(rows + " rows added!");
                                        enrolllist.Add(new Enroll(student, course, enrollmentdate));
                                    }
                                    else
                                    {
                                        throw new EnrollmentException("You cannot enroll for more than 4 courses");
                                    }
                                }
                                catch (EnrollmentException ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            else
                            {
                                throw new Repeatedexception("You have already enrolled for the course");
                            }
                        }
                        catch (Repeatedexception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else
                    {
                        throw new InvalidStudentException("Student id is not present in the database. Please register first!!");
                    }
                }
                catch(InvalidStudentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            
            catch(EnrollmentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            con.Close();
        }
        public List<Enroll> listOfEnrollments()
        {
            con.Close();
            //Console.WriteLine("Displaying from the list");
            //if (enrolllist.Count() > 0)
            //{
            //    foreach (var enroll in enrolllist)
            //    {
            //        Console.WriteLine(enroll.Student.Id + " " + enroll.Student.Name + " " + enroll.EnrollmentDate);
            //        //info.display(enroll);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("No students to display from the list");
            //}
            try
            {
                con.Open();
                Console.WriteLine("Connected successfully");
                Console.WriteLine("Displaying enrollments from the database");
                SqlCommand cmd = new SqlCommand("select * from enroll", con);
                SqlDataReader dr = cmd.ExecuteReader();
                Console.WriteLine("Records inserted into the database are: ");
                if (dr.HasRows)
                {
                    Console.WriteLine("{0}\t{1}\t{2}", dr.GetName(0), dr.GetName(1), dr.GetName(2));
                    while (dr.Read())
                    {
                        Console.WriteLine(dr.GetInt32(0) + "\t\t" + dr.GetInt32(1) + "\t\t" + dr.GetDateTime(2));
                    }
                }
                else
                {
                    Console.WriteLine("There are no rows inserted into the database");
                }
                con.Close();
                dr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public void displaystudentbyid()
        {
            try
            {
                con.Open();
                Console.WriteLine("Connected successfully");
                Console.WriteLine("Enter the id you want to filter");
                int id = Convert.ToInt32(Console.ReadLine());
                SqlCommand cmd = new SqlCommand("Select * from student_table where studentid=@id", con);
                cmd.CommandType = CommandType.Text;
                SqlParameter param = new SqlParameter("@id", id);
                cmd.Parameters.Add(param);
                SqlDataReader dr = cmd.ExecuteReader();
                try
                {
                    if (dr.Read())
                    {
                        Console.WriteLine("{0}\t{1}\t{2}", dr.GetName(0), dr.GetName(1), dr.GetName(2));
                        Console.WriteLine(dr[0] + " " + dr[1] + " " + dr[2]);
                    }
                    else
                    {
                        Console.WriteLine("No student found with that Id");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                con.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void displaycoursebyid()
        {
            try
            {
                con.Open();
                Console.WriteLine("Connected successfully");
                Console.WriteLine("Enter the id you want to filter");
                int id = Convert.ToInt32(Console.ReadLine());
                SqlCommand cmd = new SqlCommand("Select * from course where Cid=@id", con);
                cmd.CommandType = CommandType.Text;
                SqlParameter param = new SqlParameter("@id", id);
                cmd.Parameters.Add(param);
                SqlDataReader dr = cmd.ExecuteReader();
                try
                {
                    if (dr.Read())
                    {
                        Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}", dr.GetName(0), dr.GetName(1), dr.GetName(2), dr.GetName(3), dr.GetName(4), dr.GetName(5), dr.GetName(6), dr.GetName(7), dr.GetName(8));
                        Console.WriteLine(dr[0] + " " + dr[1] + " " + dr[2] + " " + dr[3] + " " + dr[4] + " " + dr[5] + " " + dr[6] + " " + dr[7] + " " + dr[8]);
                    }
                    else
                    {
                        Console.WriteLine("No course found with that id");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                con.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}