using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MISLCELib.Common;

namespace MISLCELib
{
    public class Teacher
    {
        #region Properties
        databaseconnection dbcon = new databaseconnection();
        public string TeacherID { get; set; }
        public Fullname TeacherName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string StreetNo { get; set; }
        public string Baranggay { get; set; }
        public string City { get; set; }
        public string Provice { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public string Empid { get; set; }
        public string Specialization { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public bool IsEmpty { get; set; }
        #endregion
        #region Instantiators
        /// <summary>
        /// Instantiate Blank Teacher Object
        /// </summary>
        public Teacher()
        {
            IsEmpty = true;
        }
        /// <summary>
        /// Initialize Teacher Object by teacherID
        /// </summary>
        /// <param name="teacherID"></param>
        public Teacher(string teacherID)
        {
            DataSet dset = dbcon.GetDataset("SELECT * FROM tbl_teacher WHERE teacherid='" + teacherID + "'");
            if (dset.Tables[0].Rows.Count > 0)
            {
                var row = dset.Tables[0].Rows[0];
                TeacherID = row["teacherid"].ToString();
                Fullname fullname = new Fullname();
                fullname.Fname = row["fname"].ToString();
                fullname.Mname = row["mname"].ToString();
                fullname.Lname = row["lname"].ToString();
                fullname.Suffix = row["suffix"].ToString();
                TeacherName = fullname;
                BirthDate = DateTime.Parse(row["birthdate"].ToString());
                Gender = row["gender"].ToString();
                StreetNo = row["street"].ToString();
                Baranggay = row["baranggay"].ToString();
                City = row["city"].ToString();
                Provice = row["province"].ToString();
                Email = row["email"].ToString();
                ContactNo = row["contactno"].ToString();
                Empid = row["empid"].ToString();
                Specialization = row["specialization"].ToString();
                IsDeleted = row["isdeleted"].ToString() == "1";
                IsActive = row["isactive"].ToString() == "1";
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
        /// Get Teacher List by filter
        /// </summary>
        /// <param name="filter">filter for fullname of Teacher</param>
        /// <returns></returns>
        public List<Teacher> GetTeacherList(string filter)
        {
            List<Teacher> teachers = new List<Teacher>();
            DataSet dset = dbcon.GetDataset("SELECT * FROM tbl_teacher WHERE fullname like '%" + filter + "%' and isdeleted='0'");
            if (dset.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dset.Tables[0].Rows.Count; i++)
                {
                    var row = dset.Tables[0].Rows[i];
                    Teacher teacher = new Teacher();
                    teacher.TeacherID = row["teacherid"].ToString();
                    Fullname fullname = new Fullname();
                    fullname.Fname = row["fname"].ToString();
                    fullname.Mname = row["mname"].ToString();
                    fullname.Lname = row["lname"].ToString();
                    fullname.Suffix = row["suffix"].ToString();
                    teacher.TeacherName = fullname;
                    teacher.BirthDate = DateTime.Parse(row["birthdate"].ToString());
                    teacher.Gender = row["gender"].ToString();
                    teacher.StreetNo = row["street"].ToString();
                    teacher.Baranggay = row["baranggay"].ToString();
                    teacher.City = row["city"].ToString();
                    teacher.Provice = row["province"].ToString();
                    teacher.Email = row["email"].ToString();
                    teacher.ContactNo = row["contactno"].ToString();
                    teacher.Empid = row["empid"].ToString();
                    teacher.Specialization = row["specialization"].ToString();
                    teacher.IsDeleted = row["isdeleted"].ToString() == "1";
                    teacher.IsActive = row["isactive"].ToString() == "1";
                    teacher.IsEmpty = false;
                    teachers.Add(teacher);
                }
            }
            return teachers;
        }
        /// <summary>
        /// Add Teacher
        /// </summary>
        /// <returns></returns>
        public MethodReturn AddTeacher()
        {
            MethodReturn mr = new MethodReturn();
            mr = ValidateInput();
            if (mr.IsSuccess)
            {
                if (!TeacherExist(TeacherName.GetFullname(NameFormat.FnMnLnS), Email))
                {
                    mr.IsSuccess = dbcon.Exec("INSERT INTO tbl_teacher(fullname, fname, mname, lname, suffix, birthdate, gender, street, baranggay, " +
                        "city, province, region, email, contactno, empid, specialization)" +
                        "VALUES (" +
                        "'" + TeacherName.GetFullname(NameFormat.FnMnLnS) + "', " +    //Fullname
                        "'" + TeacherName.Fname + "', " +    //Fname
                        "'" + TeacherName.Mname + "', " +    //Mname
                        "'" + TeacherName.Lname + "', " +    //Lname
                        "'" + TeacherName.Suffix + "', " +    //Suffix
                        "'" + BirthDate.ToString("yyyy-MM-dd HH:mm:ss") + "', " +    //Birthdate
                        "'" + Gender + "', " +    //Gender
                        "'" + StreetNo + "', " +    //Street
                        "'" + Baranggay + "', " +    //Baranggay
                        "'" + City + "', " +    //City
                        "'" + Provice + "', " +    //Province
                        "'', " +    //Region
                        "'" + Email + "', " +    //Email
                        "'" + ContactNo + "', " +    //ContactNo
                        "'" + Empid + "', " +    //Empid
                        "'" + Specialization + "')");      //Specialization
                    mr.Message = mr.IsSuccess ? "Successfully Added New Teacher" : "Something went wrong, please try again!";
                }
                else
                {
                    mr.IsSuccess = false;
                    mr.Message = "Teacher with name: " + TeacherName.GetFullname(NameFormat.FnMnLnS) + " Already Exist!";
                }
            }
            return mr;
        }
        /// <summary>
        /// Update Teacher Details
        /// </summary>
        /// <returns></returns>
        public MethodReturn UpdateTeacher()
        {
            MethodReturn mr = new MethodReturn();
            mr = ValidateInput();
            if (mr.IsSuccess)
            {
                if (TeacherExist(TeacherName.GetFullname(NameFormat.FnMnLnS), Email))
                {
                    mr.IsSuccess = dbcon.Exec("UPDATE tbl_teacher SET " +
                        "fullname='" + TeacherName.GetFullname(NameFormat.FnMnLnS) + "', " +
                        "fname='" + TeacherName.Fname + "', " +
                        "mname='" + TeacherName.Mname + "', " +
                        "lname='" + TeacherName.Lname + "', " +
                        "suffix='" + TeacherName.Suffix + "', " +
                        "birthdate='" + BirthDate.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                        "gender='" + Gender + "', " +
                        "streetno='" + StreetNo + "', " +
                        "baranggay='" + Baranggay + "', " +
                        "city='" + City + "', " +
                        "province='" + Provice + "', " +
                        "email='" + Email + "', " +
                        "contactno='" + ContactNo + "', " +
                        "specialization='" + Specialization + "', " +
                        "WHERE teacherid='" + TeacherID + "'");
                    mr.Message = mr.IsSuccess ? "Successfully updated Teacher details" : "Something went wrong, please try again";
                }
                else
                {
                    mr.IsSuccess = false;
                    mr.Message = "Teacher does not exist!";
                }
            }
            return mr;
        }
        /// <summary>
        /// Delete Teacher
        /// </summary>
        /// <returns></returns>
        public MethodReturn DeleteTeacher()
        {
            MethodReturn mr = new MethodReturn();
            mr = ValidateInput();
            if (mr.IsSuccess)
            {
                if (TeacherExist(TeacherName.GetFullname(NameFormat.FnMnLnS), Email))
                {
                    mr.IsSuccess = dbcon.Exec("UPDATE tbl_teacher SET " +
                        "isdeleted='" + (IsDeleted ? "1" : "0") + "', " +
                        "WHERE teacherid='" + TeacherID + "'");
                    mr.Message = mr.IsSuccess ? "Successfully Archived Teacher details" : "Something went wrong, please try again";
                }
                else
                {
                    mr.IsSuccess = false;
                    mr.Message = "Teacher does not exist!";
                }
            }
            return mr;
        }
        #endregion
        #region Private Methods
        /// <summary>
        /// Check if Teacher Exist by Teacher name
        /// </summary>
        /// <param name="teachername"></param>
        /// <returns></returns>
        public bool TeacherExist(string teachername, string email="")
        {
            return dbcon.GetDataset("SELECT * FROM tbl_teacher WHERE fullname='" + teachername + "' " + (email == "" ? "" : " and email='" + email + "'")).Tables[0].Rows.Count > 0;
        }
        /// <summary>
        /// Validate Object inputs
        /// </summary>
        /// <returns>Method Return (IsSuccess & Message)</returns>
        public MethodReturn ValidateInput()
        {
            MethodReturn mr = new MethodReturn();
            if (TeacherName.Fname == "")
            {
                mr.IsSuccess = false;
                mr.Message = "Invalid Input for Firstname";
            }
            if (TeacherName.Lname == "")
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
            if (Provice == "")
            {
                mr.IsSuccess = false;
                mr.Message = "Invalid Input for Provice";
            }
            if (Email == "")
            {
                mr.IsSuccess = false;
                mr.Message = "Invalid Input for Email";
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
