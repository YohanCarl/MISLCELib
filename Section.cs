using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MISLCELib.Common;

namespace MISLCELib
{
    public class Section
    {
        #region Properties
        databaseconnection dbcon = new databaseconnection();
        public string SectionID { get; set; }
        public string GradeNo { get; set; }
        public string SectionName { get; set; }
        public Teacher Adviser { get; set; }
        public string HomeRoom { get; set; }
        public string SchoolYear { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsEmpty { get; set; }
        #endregion
        #region Instantiator
        /// <summary>
        /// Instantiate Empty Section Object
        /// </summary>
        public Section()
        {
            IsEmpty = true;
        }
        /// <summary>
        /// Instantiate Section Object by Section ID
        /// </summary>
        /// <param name="sectionID"></param>
        public Section(string sectionID)
        {
            DataSet dset = dbcon.GetDataset("SELECT * FROM tbl_section WHERE sectionID='" + sectionID + "'");
            if (dset.Tables[0].Rows.Count > 0)
            {
                var row = dset.Tables[0].Rows[0];
                SectionID = row["SectionID"].ToString();
                GradeNo = row["gradeNo"].ToString();
                SectionName = row["SectionName"].ToString();
                Teacher teacher = new Teacher(row["adviser"].ToString());
                Adviser = teacher;
                HomeRoom = row["homeroom"].ToString();
                SchoolYear = row["schoolyear"].ToString();
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
        public List<Section> GetSectionList(string schoolyear, string filter="*")
        {
            List<Section> sections = new List<Section>();
            string where = filter == "*" ? "" : " WHERE (sectionname='" + filter + "' OR gradeno='" + filter + "') AND schoolyear ='" + schoolyear + "' ";
            DataSet dset = dbcon.GetDataset("SELECT * FROM tbl_section" + where);
            if (dset.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dset.Tables[0].Rows.Count; i++)
                {
                    var row = dset.Tables[0].Rows[i];
                    Section section = new Section();
                    section.SectionID = row["SectionID"].ToString();
                    section.GradeNo = row["gradeNo"].ToString();
                    section.SectionName = row["SectionName"].ToString();
                    Teacher teacher = new Teacher(row["adviser"].ToString());
                    section.Adviser = teacher;
                    section.HomeRoom = row["homeroom"].ToString();
                    section.SchoolYear = row["schoolyear"].ToString();
                    section.IsActive = row["isactive"].ToString() == "1";
                    section.IsDeleted = row["isdeleted"].ToString() == "1";
                    section.IsEmpty = false;
                    sections.Add(section);
                }
            }
            return sections;
        }
        public MethodReturn AddSection()
        {
            MethodReturn mr = new MethodReturn();
            mr = ValidateInput();
            if (mr.IsSuccess)
            {
                if (!SectionExist(SectionName,SchoolYear))
                {
                    mr.IsSuccess = dbcon.Exec("INSERT INTO tbl_section(gradeno, sectionname, adviser, homeroom, schoolyear)" +
                        "Values(" +
                        "'" + GradeNo + "', " +    //GradeNo
                        "'" + SectionName + "', " +    //Section Name
                        "'" + Adviser.TeacherID + "', " +    //Adviser(teacherID)
                        "'" + HomeRoom + "', " +    //Home Room
                        "'" + SchoolYear + "')");      //School Year
                    mr.Message = mr.IsSuccess ? "Successfully Enrolled new Section" : "Something went wrong, please try again";
                }
                else
                {
                    mr.IsSuccess = false;
                    mr.Message = "Section Already Exist!";
                }
            }
            return mr;
        }
        public MethodReturn EditSection()
        {
            MethodReturn mr = new MethodReturn();
            mr = ValidateInput();
            if (mr.IsSuccess)
            {
                if (SectionExist(SectionName, SchoolYear))
                {
                    mr.IsSuccess = dbcon.Exec("UPDATE tbl_section SET " +
                        "GradeNo='" + GradeNo + "', " +
                        "SectionName='" + SectionName + "', " +
                        "Adviser='" + Adviser.TeacherID + "', " +
                        "Homeroom='" + HomeRoom + "', " +
                        "SchoolYear='" + SchoolYear + "' " +
                        "WHERE sectionID='" + SectionID + "'");
                    mr.Message = mr.IsSuccess ? "Successfully update Section" : "Something went wrong please try again";
                }
                else
                {
                    mr.IsSuccess = false;
                    mr.Message = "Section does not exist!";
                }
            }
            return mr;
        }
        public MethodReturn DeleteSection()
        {
            MethodReturn mr = new MethodReturn();
            mr = ValidateInput();
            if (mr.IsSuccess)
            {
                if (SectionExist(SectionName, SchoolYear))
                {
                    mr.IsSuccess = dbcon.Exec("UPDATE tbl_section SET isdeleted=1 WHERE sectionID='" + SectionID + "'");
                    mr.Message = mr.IsSuccess ? "Successfully Archived Section" : "Something went wrong, please try again";
                }
                else
                {
                    mr.IsSuccess = false;
                    mr.Message = "Section does not exist!";
                }
            }
            return mr;
        }
        #endregion
        #region Private Methods
        private bool SectionExist(string sectionname, string schoolyear)
        {
            return dbcon.GetDataset("SELECT * FROM tbl_section WHERE sectionname='" + sectionname + "' and schoolyear='" + schoolyear + "'").Tables[0].Rows.Count > 0;
        }
        private MethodReturn ValidateInput()
        {
            MethodReturn mr = new MethodReturn();
            mr.IsSuccess = true;
            if (GradeNo == "")
            {
                mr.IsSuccess = false;
                mr.Message = "Invalid input for Grade No.";
            }
            if (SectionName == "")
            {
                mr.IsSuccess = false;
                mr.Message = "Invalid input for Section Name";
            }
            if (HomeRoom=="")
            {
                mr.IsSuccess = false;
                mr.Message = "Invalid input for Home Room";
            }
            if (SchoolYear == "")
            {
                mr.IsSuccess = false;
                mr.Message = "Invalid input for School Year";
            }
            if (Adviser.IsEmpty)
            {
                mr.IsSuccess = false;
                mr.Message = "Invalid input for Adviser";
            }
            return mr;
        }
        #endregion
    }
}
