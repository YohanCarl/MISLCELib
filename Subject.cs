using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MISLCELib.Common;

namespace MISLCELib
{
    public class Subject
    {
        #region Properties
        databaseconnection dbcon = new databaseconnection();
        public string SubjectID { get; set; }
        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectDescription {  get; set; }
        public Teacher SubjectTeacher { get; set; }
        public string SchoolYear { get; set; }
        public string GradeLevel { get; set; }
        public bool IsEmpty { get; set; }
        #endregion
        #region Instantiator
        public Subject()
        {
            IsEmpty = true;
        }
        public Subject(string subjectID)
        {
            DataSet dset = dbcon.GetDataset("SELECT * FROM tbl_subject WHERE subjectID='" + subjectID + "'");
            if (dset.Tables[0].Rows.Count > 0)
            {
                var row = dset.Tables[0].Rows[0];
                SubjectID = row["subjectID"].ToString();
                SubjectName = row["subjectName"].ToString();
                SubjectCode = row["subjectCode"].ToString();
                SubjectDescription = row["subjectDescription"].ToString();
                Teacher teacher = new Teacher(row["teacherID"].ToString());
                SubjectTeacher = teacher;
                SchoolYear = row["SchoolYear"].ToString();
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
        #endregion
        #region Private Method
        #endregion
    }
}
