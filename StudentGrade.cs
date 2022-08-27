using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MISLCELib.Common;

namespace MISLCELib
{
    public class StudentGrade
    {
        #region Properties
        databaseconnection dbcon = new databaseconnection();
        public string StgdID  { get; set; }
        public string StudentID { get; set; }
        public string SubjectID { get; set; }
        public string FirstQuarter { get; set; }
        public string SecondQuarter { get; set; }
        public string ThirdQuarter { get; set; }
        public string FourthQuater { get; set; }
        public string FinalRating { get; set; }
        public string SchoolYear { get; set; }
        public bool IsPassed { get; set; }
        public bool IsDropped { get; set; }
        public bool IsEmpty { get; set; }
        #endregion
        #region Instantiator
        /// <summary>
        /// Instantiate Empty Student Grade Object
        /// </summary>
        public StudentGrade()
        {
            IsEmpty = true;
        }
        /// <summary>
        /// Instantiate Student Grade Object by Student Grade ID
        /// </summary>
        /// <param name="stgdid"></param>
        public StudentGrade(string stgdid)
        {
            DataSet dset = dbcon.GetDataset("SELECT * FROM tbl_student_grade WHERE stdgdID='" + stgdid + "'");
            if (dset.Tables[0].Rows.Count > 0)
            {
                var row = dset.Tables[0].Rows[0];
                StgdID = row["stdgID"].ToString();
                StudentID = row["studentID"].ToString();
                SubjectID = row["subjectID"].ToString();
                FirstQuarter = row["firstquarter"].ToString();
                SecondQuarter = row["secondquarter"].ToString();
                ThirdQuarter = row["thirdquarted"].ToString();
                FourthQuater = row["fourthquarter"].ToString();
                FinalRating = row["finalrating"].ToString();
                SchoolYear = row["schoolyear"].ToString();
                IsPassed = row["ispassed"].ToString() == "1";
                IsDropped = row["isdropped"].ToString() == "1";
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
        /// Get Student GradeList
        /// </summary>
        /// <param name="studentID"></param>
        /// <param name="schoolyear"></param>
        /// <returns></returns>
        public List<StudentGrade> GetStudentGrades(string studentID, string schoolyear)
        {
            List<StudentGrade> studentGrades = new List<StudentGrade>();
            DataSet dset = dbcon.GetDataset("SELECT * FROM tbl_student_grade WHERE studentID='" + studentID + "' AND schoolyear='" + schoolyear + "'");
            if (dset.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dset.Tables[0].Rows.Count; i++)
                {
                    var row = dset.Tables[0].Rows[i];
                    StudentGrade studentGrade = new StudentGrade();
                    studentGrade.StgdID = row["stdgID"].ToString();
                    studentGrade.StudentID = row["studentID"].ToString();
                    studentGrade.SubjectID = row["subjectID"].ToString();
                    studentGrade.FirstQuarter = row["firstquarter"].ToString();
                    studentGrade.SecondQuarter = row["secondquarter"].ToString();
                    studentGrade.ThirdQuarter = row["thirdquarted"].ToString();
                    studentGrade.FourthQuater = row["fourthquarter"].ToString();
                    studentGrade.FinalRating = row["finalrating"].ToString();
                    studentGrade.SchoolYear = row["schoolyear"].ToString();
                    studentGrade.IsPassed = row["ispassed"].ToString() == "1";
                    studentGrade.IsDropped = row["isdropped"].ToString() == "1";
                    studentGrade.IsEmpty = false;
                    studentGrades.Add(studentGrade);
                }
            }
            return studentGrades;
        }
        /// <summary>
        /// Encode Student Grade on SubjectID
        /// </summary>
        /// <returns></returns>
        public MethodReturn EncodeStudentGrade(GradingPeriod gradingPeriod = GradingPeriod.FirstQuarter)
        {
            MethodReturn mr = new MethodReturn();
            if (StudentGradeExist(StudentID, SubjectID, SchoolYear))
            {
                mr.IsSuccess = dbcon.Exec("UPDATE tbl_student_grade SET " +
                    "firstquarter='" + FirstQuarter + "', " +
                    "secondquarter='" + SecondQuarter + "', " +
                    "thirdquarter='" + ThirdQuarter + "', " +
                    "fourthquarter='" + FourthQuater + "', " +
                    "finalrating='" + FinalRating + "', " +
                    "IsPassed='" + (IsPassed ? "1" : "0") + "', " +
                    "IsDropped='" + (IsDropped ? "1" : "0") + "' " +
                    "WHERE studentID='" + StudentID + "' AND subjetID='" + SubjectID + "' AND schoolyear='" + SchoolYear + "'");
                mr.Message = mr.IsSuccess ? "Succesfully Encoded Student Grade" : "Something went wrong, please try again!";
            }
            else
            {
                mr.IsSuccess = dbcon.Exec("INSERT INTO tbl_student_grade(studentID, subjectID, firstquarter, secondquarter, thirdquarter, fourthquarter, finalrating, schoolyear, ispassed, isdropped) VALUES(" +
                    "'" + StudentID + "', " +
                    "'" + SubjectID + "', " +
                    "'" + FirstQuarter + "', " +
                    "'" + SecondQuarter + "', " +
                    "'" + ThirdQuarter + "', " +
                    "'" + FourthQuater + "', " +
                    "'" + FinalRating + "', " +
                    "'" + SchoolYear + "', " +
                    "'0'," +
                    "'0')");
                mr.Message = mr.IsSuccess ? "Succesfully Encoded New Student Grade" : "Something went wrong, please try again!";
            }
            return mr;
        }
        /// <summary>
        /// Get Student Grade details and drop student on subject
        /// </summary>
        /// <returns></returns>
        public MethodReturn DropStudent()
        {
            MethodReturn mr = new MethodReturn();
            if (StudentGradeExist(StudentID, SubjectID, SchoolYear))
            {
                mr.IsSuccess = dbcon.Exec("UPDATE tbl_student_grade SET " +
                    "isdropped='1'" +
                    "WHERE stdgID='" + StgdID + "'");
                mr.Message = mr.IsSuccess ? "Successfully dropped student on subject" : "Something went wrong, please try again!";
            }
            else
            {
                mr.IsSuccess = false;
                mr.Message = "Student Record does not exist!";
            }
            return mr;
        }
        #endregion
        #region Private Methods
        private bool StudentGradeExist(string studentID, string subjectID, string schoolyear)
        {
            return dbcon.GetDataset("SELECT * FROM tbl_student_grade " +
                "WHERE studentID='" + studentID + "' " +
                "AND subjectID='" + subjectID + "' " +
                "AND schoolyear='" + schoolyear + "'").Tables[0].Rows.Count > 0;
        }
        #endregion
    }
    public enum GradingPeriod
    {
        FirstQuarter,
        SecondQuarter,
        ThirdQuarter,
        FourthQuater
    }
}
