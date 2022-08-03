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

        #endregion
        #region Private Methods

        #endregion
    }
}
