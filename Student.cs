using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MISLCELib.Common;


namespace MISLCELib
{
    public class Student
    {
        #region Properties
        databaseconnection dbcon = new databaseconnection();
        public string StudentID { get; set; }
        public Fullname StudentName { get; set; }
        public DateTime Birthdate { get; set; }
        public string Gender { get; set; }
        public string MothersName { get; set; }
        public string FathersName { get; set; }
        public string StreetNo { get; set; }
        public string Baranggay { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Guardian { get; set; }
        public string LRN { get; set; }
        public string ContactNo { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Section Section { get; set; }
        public bool IsEmpty { get; set; }
        #endregion
        #region Instantiator
        /// <summary>
        /// Instantiate Empty Student Object
        /// </summary>
        public Student()
        {
            IsEmpty = true;
            Birthdate = DateTime.Now;
        }
        /// <summary>
        /// Instantiate Studnet Object using ID
        /// </summary>
        /// <param name="studentID"></param>
        public Student(string studentID)
        {
            DataSet dset = dbcon.GetDataset("SELECT * FROM tbl_students WHERE studentID='" + studentID + "'");
            if (dset.Tables[0].Rows.Count > 0)
            {
                var row = dset.Tables[0].Rows[0];
                StudentID = row["studentID"].ToString();
                Fullname fullname = new Fullname();
                fullname.Fname = row["fname"].ToString();
                fullname.Lname = row["lname"].ToString();
                fullname.Mname = row["mname"].ToString();
                fullname.Suffix = row["suffix"].ToString();
                StudentName = fullname;
                Birthdate = DateTime.Parse(row["birthdate"].ToString());
                Gender = row["gender"].ToString();
                MothersName = row["mothersname"].ToString();
                FathersName = row["fathername"].ToString();
                StreetNo = row["street"].ToString();
                Baranggay = row["baranggay"].ToString();
                City = row["city"].ToString();
                Province = row["province"].ToString();
                LRN = row["lrn"].ToString();
                Guardian = row["guardian"].ToString();
                ContactNo = row["contactinfo"].ToString();
                IsActive = row["isactive"].ToString() == "1";
                IsDeleted = row["isdeleted"].ToString() == "1";
                IsEmpty = false;
            }
            else
            {
                IsEmpty = true;
            }
        }
        #endregion
        #region Public Methods
        /// <summary>
        /// Get the List of Student by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<Student> GetStudentList(string filter)
        {
            List<Student> students = new List<Student>();
            DataSet dset = dbcon.GetDataset("SELECT * FROM tbl_students WHERE fullname like ='%"+filter+"%'");
            if (dset.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dset.Tables[0].Rows.Count; i++)
                {
                    var row = dset.Tables[0].Rows[i];
                    Student student = new Student();
                    student.StudentID = row["studentID"].ToString();
                    Fullname fullname = new Fullname();
                    fullname.Fname = row["fname"].ToString();
                    fullname.Lname = row["lname"].ToString();
                    fullname.Mname = row["mname"].ToString();
                    fullname.Suffix = row["suffix"].ToString();
                    student.StudentName = fullname;
                    student.Birthdate = DateTime.Parse(row["birthdate"].ToString());
                    student.Gender = row["gender"].ToString();
                    student.MothersName = row["mothersname"].ToString();
                    student.FathersName = row["fathername"].ToString();
                    student.StreetNo = row["street"].ToString();
                    student.Baranggay = row["baranggay"].ToString();
                    student.City = row["city"].ToString();
                    student.Province = row["province"].ToString();
                    student.LRN = row["lrn"].ToString();
                    student.Guardian = row["guardian"].ToString();
                    student.ContactNo = row["contactinfo"].ToString();
                    student.IsActive = row["isactive"].ToString() == "1";
                    student.IsDeleted = row["isdeleted"].ToString() == "1";
                    student.IsEmpty = false;
                    students.Add(student);
                }
            }
            return students;
        }
        /// <summary>
        /// Add New Student
        /// </summary>
        /// <returns>If Successful and Message</returns>
        public MethodReturn AddStudent()
        {
            MethodReturn mr = new MethodReturn();
            mr = ValidateInput();
            if (mr.IsSuccess)
            {
                if (!StudentExist(StudentName.GetFullname(NameFormat.FnMnLnS), LRN))
                {
                    mr.IsSuccess = dbcon.Exec("INSERT INTO tbl_students (fullname, fname, lname, mname, suffix, birthdate, gender, " +
                        "mothersname, fathername, street, baranggay, city, province, region, lrn, guardian, contcatinfo) VALUES (" +
                        "'" + StudentName.GetFullname() + "', " +    //Fullname
                        "'" + StudentName.Fname + "', " +    //Fname
                        "'" + StudentName.Lname + "', " +    //Lname
                        "'" + StudentName.Mname + "', " +    //Mname
                        "'" + StudentName.Suffix + "', " +    //Suffix
                        "'" + Birthdate.ToString("yyyy-MM-dd HH:mm:ss") + "', " +    //Birthdate
                        "'" + Gender + "', " +    //Gender
                        "'" + MothersName + "', " +    //Mothers Name
                        "'" + FathersName + "', " +    //Fathers Name
                        "'" + StreetNo + "', " +    //Street
                        "'" + Baranggay + "', " +    //Baranggay
                        "'" + City + "', " +    //City
                        "'" + Province + "', " +    //Province
                        "'', " +    //Region
                        "'" + LRN + "', " +    //LRN
                        "'" + Guardian + "', " +    //Guardian
                        "'" + ContactNo + "' ");      //contactinfo
                    mr.Message = mr.IsSuccess ? "Successfully Added new Student!" : "Something went wrong please try again";
                }
                else
                {
                    mr.IsSuccess = false;
                    mr.Message = "Student already exist!";
                }
            }
            return mr;
        }
        /// <summary>
        /// Edit Student Details
        /// </summary>
        /// <returns></returns>
        public MethodReturn EditStudent()
        {
            MethodReturn mr = new MethodReturn();
            mr = ValidateInput();
            if (mr.IsSuccess)
            {
                if (StudentExist(StudentName.GetFullname(), LRN))
                {
                    mr.IsSuccess = dbcon.Exec("UPDATE tbl_students SET " +
                        "fullname='" + StudentName.GetFullname(NameFormat.FnMnLnS) + "', " +
                        "fname='" + StudentName.Fname + "', " +
                        "lname='" + StudentName.Lname + "', " +
                        "mname='" + StudentName.Mname + "', " +
                        "suffix='" + StudentName.Suffix + "', " +
                        "birthdate='" + Birthdate.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                        "gender='" + Gender + "', " +
                        "mothersname='" + MothersName + "', " +
                        "fathername='" + FathersName + "', " +
                        "street='" + StreetNo + "', " +
                        "baranggay='" + Baranggay + "'," +
                        "city='" + City + "'," +
                        "province='" + Province + "'," +
                        "guradian='" + Guardian + "', " +
                        "contactinfo='" + ContactNo + "' " +
                        "WHERE studentID='" + StudentID + "'");
                    mr.Message = mr.IsSuccess ? "Successfully updated Student Information" : "Something went wrong, please try again";
                }
                else
                {
                    mr.IsSuccess = false;
                    mr.Message = "No such student exist!";
                }
            }
            return mr;
        }
        /// <summary>
        /// Archive Student in table.
        /// </summary>
        /// <returns></returns>
        public MethodReturn DeleteStudent()
        {
            MethodReturn mr = new MethodReturn();
            mr = ValidateInput();
            if (mr.IsSuccess)
            {
                if (StudentExist(StudentName.GetFullname(), LRN))
                {
                    mr.IsSuccess = dbcon.Exec("UPDATE tbl_students SET isdeleted='1' WHERE studentID='" + StudentID + "'");
                    mr.Message = mr.IsSuccess ? "Successfully archive student" : "Something went wrong, please try again";
                }
                else
                {
                    mr.IsSuccess = false;
                    mr.Message = "No such student exist!";
                }
            }
            return mr;
        }
        #endregion
        #region Private Methods
        /// <summary>
        /// Checks if Student Exist
        /// </summary>
        /// <param name="fullname"></param>
        /// <param name="lrn"></param>
        /// <returns></returns>
        public bool StudentExist(string fullname, string lrn)
        {
            return dbcon.GetDataset("SELECT * FROM tbl_students WHERE fullname='"+fullname+"' and lrn='" + lrn + "'").Tables[0].Rows.Count > 0;
        }
        /// <summary>
        /// Validate Inputs
        /// </summary>
        /// <returns></returns>
        public MethodReturn ValidateInput()
        {
            MethodReturn mr = new MethodReturn();
            mr.IsSuccess = true;
            if (StudentName.Fname == "")
            {
                mr.IsSuccess = false;
                mr.Message = "Invalid Input for Firstname";
            }
            if (StudentName.Lname == "")
            {
                mr.IsSuccess = false;
                mr.Message = "Invalid Input for Lastname";
            }
            if (StreetNo == "")
            {
                mr.IsSuccess = false;
                mr.Message = "Invalid Input for Street No";
            }
            if (Baranggay == "")
            {
                mr.IsSuccess = false;
                mr.Message = "Invalid Input for Baranggay";
            }
            if (City == "")
            {
                mr.IsSuccess = false;
                mr.Message = "Invalid Input for City";
            }
            if (Province == "")
            {
                mr.IsSuccess = false;
                mr.Message = "Invalid Input for Provice";
            }
            if (ContactNo == "")
            {
                mr.IsSuccess = false;
                mr.Message = "Invalid Input for ContactNo";
            }
            return mr;
        }
        #endregion
    }
}
