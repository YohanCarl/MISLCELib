using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MISLCELib.Common;

namespace MISLCELib
{
    public class StudentHistory
    {
        #region Properties
        databaseconnection dbcon = new databaseconnection();
        public string Sthid { get; set; }
        public string StudentID { get; set; }
        public string SchoolYear { get; set; }
        public string SectionID { get; set; }
        public bool IsPassed { get; set; }
        public string GradeLevel { get; set; }
        public bool IsEmpty { get; set; }
        #endregion
        #region Instantiator
        /// <summary>
        /// Instatiate an Empty Student History object
        /// </summary>
        public StudentHistory()
        {
            IsEmpty = true;
        }
        /// <summary>
        /// Instantiate Studnet History Object by student history ID
        /// </summary>
        /// <param name="sthid"></param>
        public StudentHistory(string sthid)
        {
            DataSet dset = dbcon.GetDataset("SELECT * FROM tbl_student_history WHERE sthid='" + sthid + "'");
            if (dset.Tables[0].Rows.Count > 0)
            {
                var row = dset.Tables[0].Rows[0];
                Sthid = row["sthid"].ToString();
                StudentID = row["studentID"].ToString();
                SchoolYear = row["schoolyear"].ToString();
                SectionID = row["sectionID"].ToString();
                IsPassed = row["ispassed"].ToString() == "1";
                GradeLevel = row["gradelevel"].ToString();
                IsEmpty = false;
            }
            else
            {
                IsEmpty = true;
            }
        }
        #endregion
        #region Public Method
        /// <summary>
        /// Get Studnet's history
        /// </summary>
        /// <param name="studentID"></param>
        /// <returns></returns>
        public List<StudentHistory> GetStudentHistories(string studentID)
        {
            List<StudentHistory> studentHistories = new List<StudentHistory>();
            DataSet dset = dbcon.GetDataset("SELECT * FROM tbl_student_history WHERE studentID='" + studentID + "' ORDER BY gradelevel desc");
            if (dset.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dset.Tables[0].Rows.Count; i++)
                {
                    var row = dset.Tables[0].Rows[i];
                    StudentHistory studentHistory = new StudentHistory();
                    studentHistory.Sthid = row["sthid"].ToString();
                    studentHistory.StudentID = row["studentID"].ToString();
                    studentHistory.SchoolYear = row["schoolyear"].ToString();
                    studentHistory.SectionID = row["sectionID"].ToString();
                    studentHistory.IsPassed = row["ispassed"].ToString() == "1";
                    studentHistory.GradeLevel = row["gradelevel"].ToString();
                    studentHistory.IsEmpty = false;
                    studentHistories.Add(studentHistory);
                }
            }
            return studentHistories;
        }
        /// <summary>
        /// Add Student history
        /// </summary>
        /// <returns></returns>
        public MethodReturn AddStudentHistory()
        {
            MethodReturn mr = new MethodReturn();
            mr = ValidateInput();
            if (mr.IsSuccess)
            {
                mr.IsSuccess = dbcon.Exec("INSERT INTO tbl_student_history (studentID, schoolyear, sectionID, gradelevel) " +
                    "VALUE(" +
                    "'" + StudentID + "', " +    //Student ID
                    "'" + SchoolYear + "', " +    //School Year
                    "'" + SectionID + "', " +    //Section ID
                    "'" + GradeLevel + "')");      //Grade Level
                mr.Message = mr.IsSuccess ? "Successfully recorded student record" : "Something went wrong please try again";
            }
            return mr;
        }
        /// <summary>
        /// Edit Student history
        /// </summary>
        /// <returns></returns>
        public MethodReturn EditStudentHistory()
        {
            MethodReturn mr = new MethodReturn();
            mr = ValidateInput();
            if (mr.IsSuccess)
            {
                if (StudentHistoryExist(Sthid))
                {
                    mr.IsSuccess = dbcon.Exec("UPDATE tbl_student_history SET " +
                        "schoolyear='" + SchoolYear + "', " +
                        "sectionID='" + SectionID + "', " +
                        "gradelevel='" + GradeLevel + "' " +
                        "WHERE sthid='" + Sthid + "'");
                    mr.Message = mr.IsSuccess ? "Successfully update studendt record!" : "Something went wrong, please try again.";
                }
                else
                {
                    mr.IsSuccess = false;
                    mr.Message = "Invalid Student history!";
                }
            }
            return mr;
        }
        #endregion
        #region Private Method
        private bool StudentHistoryExist(string sthid)
        {
            return dbcon.GetDataset("SELECT * FROM tbl_student_history WHERE sthid='" + sthid + "'").Tables[0].Rows.Count > 0;
        }
        private MethodReturn ValidateInput()
        {
            MethodReturn mr = new MethodReturn();
            mr.IsSuccess = true;
            if (StudentID == "")
            {
                mr.IsSuccess = false;
                mr.Message = "Invalid Input for Student";
            }
            if (SchoolYear == "")
            {
                mr.IsSuccess = false;
                mr.Message = "Invalid Input for SchoolYear";
            }
            if (SectionID == "")
            {
                mr.IsSuccess = false;
                mr.Message = "Invalid Input for Section";
            }
            if (GradeLevel == "")
            {
                mr.IsSuccess = false;
                mr.Message = "Invalid Input for Grade Level";
            }
            return mr;
        }
        #endregion
    }
}
