using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

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
        }
        #endregion
        #region Public Methods
        #endregion
        #region Private Methods
        private bool SectionExist(string sectionname, string schoolyear)
        {

        }
        #endregion
    }
}
