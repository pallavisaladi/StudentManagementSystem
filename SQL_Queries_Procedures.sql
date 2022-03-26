CREATE Procedure spInsertCourse
	@Cid int,
	@Cname varchar(20),
	@Cduration varchar(10),
	@Cfees money,
	@Ccourse varchar(10),
	@Clevel varchar(10),
	@Cplacement varchar(5),	
	@Ctype varchar(10),
	@Monthlyfee money
as
Begin
	Insert into course(Cid,Cname,Cduration,Cfees,Ccourse,Clevel,Cplacement,Ctype,Monthlyfee) values(@Cid,@Cname,@Cduration,@Cfees,@Ccourse,@Clevel,@Cplacement,@Ctype,@Monthlyfee)
End

CREATE procedure spInsertenroll
@studentid int, 
@courseid int,
@enrollmentdate date as
begin insert into enroll(studentid,courseid,enrollmentdate) values(@studentid,@courseid,@enrollmentdate)
end

CREATE procedure [dbo].[spInsertStudent]
@studentid int,
@studentname varchar(20),
@dob date as
begin insert into student_table(studentid,studentname,dob) values(@studentid,@studentname,@dob)
end