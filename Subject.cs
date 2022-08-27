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
        public List<Subject> GetSubjectListBySchoolYear(string schoolyear)
        {
            List<Subject> subjects = new List<Subject>();
            DataSet dset = dbcon.GetDataset("SELECT * FROM tbl_subjects WHERE SchoolYear='" + schoolyear + "'");
            if (dset.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dset.Tables[0].Rows.Count; i++)
                {
                    var row = dset.Tables[0].Rows[i];
                    Subject subject = new Subject();
                    subject.SubjectID = row["subjectID"].ToString();
                    subject.SubjectName = row["subjectName"].ToString();
                    subject.SubjectCode = row["subjectCode"].ToString();
                    subject.SubjectDescription = row["subjectDescription"].ToString();
                    Teacher teacher = new Teacher(row["teacherID"].ToString());
                    subject.SubjectTeacher = teacher;
                    subject.SchoolYear = row["SchoolYear"].ToString();
                    subject.GradeLevel = row["gradelevel"].ToString();
                    subject.IsEmpty = false;
                    subjects.Add(subject);
                }
            }
            return subjects;
        }
        public MethodReturn AddSubject()
        {
            MethodReturn mr = new MethodReturn();
            return mr;
        }
        #endregion
        #region Private Method
        private bool SubjectExist(string subjectcode)
        {
            return dbcon.GetDataset("SELECT * FROM tbl_subject where subjectcode='" + subjectcode + "'").Tables[0].Rows.Count > 0;
        }
        private MethodReturn ValidateInput()
        {
            MethodReturn mr = new MethodReturn();
            mr.IsSuccess = true;
            if (SubjectName == "")
            {
                mr.IsSuccess = false;
                mr.Message = "Invalid input for SubjectName";
            }
            if (SubjectCode == "")
            {
                mr.IsSuccess = false;
                mr.Message = "Invalid input for SubjectCode";
            }
            if (SubjectTeacher.IsEmpty)
            {
                mr.IsSuccess = false;
                mr.Message = "Invalid input for Subject Teacher";
            }
            if (SchoolYear == "")
            {
                mr.IsSuccess = false;
                mr.Message = "Invalid input for School Year";
            }
            if (GradeLevel == "")
            {
                mr.IsSuccess = false;
                mr.Message = "Invalid input for Grade Level";
            }
            return mr;
        }
        #endregion
    }
}
