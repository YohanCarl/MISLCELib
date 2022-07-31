using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MISLCELib.Common;

namespace MISLCELib
{
    public class User
    {
        #region Properties
        databaseconnection dbcon = new databaseconnection();
        public string UID { get; set; }
        public string Username { get; set; }
        public UserAccessType UserAccessType { get; set; }
        public string UPasswordRaw { get; set; }
        public string UPasswordEnc { get; set; }
        public string STEMPID { get; set; }
        public object UserDetails { get; set; }
        public bool IsLocked { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsEmpty { get; set; }
        #endregion
        #region Instancetiator
        /// <summary>
        /// Instantiate a user object using username and password
        /// Can be used as login
        /// </summary>
        /// <param name="uname">string username</param>
        /// <param name="upwd">raw unencrypted</param>
        public User(string uname, string upwd)
        {
            DataSet dset = dbcon.GetDataset("SELECT * FROM tbl_users where uname='"+uname+"' and upwd=MD5('"+upwd+"')");
            if (dset.Tables[0].Rows.Count > 0)
            {
                IsEmpty = false;
                var row = dset.Tables[0].Rows[0];
                UID = row["uid"].ToString();
                Username = row["uname"].ToString();
                UPasswordRaw = upwd;
                UPasswordEnc = row["upwd"].ToString();
                UserAccessType = ParseUserAccessType(row["accesstype"].ToString());
                IsLocked = row["islocked"].ToString() == "1";
                IsActive = row["isactive"].ToString() == "1";
                IsDeleted = row["isdeleted"].ToString() == "1";
                STEMPID = row["stempid"].ToString();
            }
            else
            {
                IsEmpty = true;
            }
        }
        /// <summary>
        /// New Empty User model
        /// </summary>
        public User()
        {
            IsEmpty = true;
        }
        /// <summary>
        /// Get User Details by ID
        /// </summary>
        /// <param name="uid"></param>
        public User(string uid)
        {
            DataSet dset = dbcon.GetDataset("SELECT * FROM tbl_users where uid='" + uid + "'");
            if (dset.Tables[0].Rows.Count > 0)
            {
                IsEmpty = false;
                var row = dset.Tables[0].Rows[0];
                UID = row["uid"].ToString();
                Username = row["uname"].ToString();
                UPasswordRaw = upwd;
                UPasswordEnc = row["upwd"].ToString();
                UserAccessType = ParseUserAccessType(row["accesstype"].ToString());
                IsLocked = row["islocked"].ToString() == "1";
                IsActive = row["isactive"].ToString() == "1";
                IsDeleted = row["isdelted"].ToString() == "1";
                STEMPID = row["stempid"].ToString();
            }
            else
            {
                IsEmpty = true;
            }
        }
        #endregion
        #region User Related methods
        /// <summary>
        /// Parse User accestype
        /// </summary>
        /// <param name="accesstype">accesstype in string</param>
        /// <returns></returns>
        public UserAccessType ParseUserAccessType(string accesstype)
        {
            switch (accesstype)
            {
                case "Admin":
                    return UserAccessType.Admin;
                    break;
                case "Teacher":
                    return UserAccessType.Teacher;
                    break;
                case "Student":
                    return UserAccessType.Student;
                case "Registrar":
                    return UserAccessType.Registrar;
                case "Accounting":
                    return UserAccessType.Accounting;
                default:
                    return UserAccessType.Invalid;
                    break;
            }
        }
        /// <summary>
        /// Check if user Exist by Username
        /// </summary>
        /// <param name="uname">Username</param>
        /// <returns></returns>
        private bool UserExist(string uname)
        {
            return dbcon.GetDataset("SELECT * FROM tbl_users where uname='" + uname + "'").Tables[0].Rows > 0;
        }

        private MethodReturn ValidateInput()
        {
            MethodReturn mr = new MethodReturn();
            if (Username=="")
            {
                mr.Message = "Invalid Username!";
                mr.IsSuccess = false;
            }
            if (UPasswordRaw == "")
            {
                mr.Message = "Invalid Password!";
                mr.IsSuccess = false;
            }
            if (UserAccessType == null)
            {
                mr.Message = "Invalid userAccessType!";
                mr.IsSuccess = false;
            }
            return mr;
        }
        /// <summary>
        /// Add new user
        /// </summary>
        /// <returns></returns>
        public MethodReturn AddUser()
        {
            MethodReturn mr = ValidateInput();
            if (mr.IsSuccess)
            {
                // Check if User Exist
                if (UserExist(Username))
                {
                    mr.Message = "User Already Exist!";
                    mr.IsSuccess = false;
                }
                // Proceed Add
                else
                {
                    mr.IsSuccess = dbcon.Exec("INSERT INTO tbl_users(uname, upwd, accesstype, islocked, stempid) VALUES (" +
                        "'"+Username+"'," +
                        "MD5('"+UPasswordRaw+"')," +
                        "'"+UserAccessType.ToString()+"'," +
                        "'"+(IsLocked ? "1" : "0")+"'," +
                        "'"+STEMPID+"')");
                    mr.Message = mr.IsSuccess ? "Successfully enrolled new user!" : "Something went wrong, please try again";
                }
            }
            return mr;
        }

        /// <summary>
        /// Get User List by filter
        /// </summary>
        /// <param name="filter">filter to username</param>
        /// <returns></returns>
        public List<User> GetUserList(string filter)
        {
            List<User> users = new List<User>();
            DataSet dset = dbcon.GetDataset("SELECT * FROM tbl_users where uname='" + uname + "'");
            if (dset.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    IsEmpty = false;
                    var row = dset.Tables[0].Rows[i];
                    User user = new User();
                    user.UID = row["uid"].ToString();
                    user.Username = row["uname"].ToString();
                    user.UserAccessType = ParseUserAccessType(row["accesstype"].ToString());
                    user.IsLocked = row["islocked"].ToString() == "1";
                    user.IsActive = row["isactive"].ToString() == "1";
                    user.IsDeleted = row["isdeleted"].ToString() == "1";
                    user.STEMPID = row["stempid"].ToString();
                    users.Add(user);
                }
            }
            else
            {
                IsEmpty = true;
            }
            return users;
        }
        
        /// <summary>
        /// Edit User
        /// </summary>
        /// <returns></returns>
        public MethodReturn EditUser()
        {
            MethodReturn mr = ValidateInput();
            if (mr.IsSuccess)
            {
                // Check if User Exist
                if (!UserExist(Username))
                {
                    mr.Message = "User Does not Exist!";
                    mr.IsSuccess = false;
                }
                // Proceed Add
                else
                {
                    mr.IsSuccess = dbcon.Exec("UPDATE tbl_users SET " +
                        "accesstype='', " +
                        "uname='' " +
                        "WHERE uid='" + UID + "'");
                    mr.Message = mr.IsSuccess ? "Successfully updated user info!" : "Something went wrong, please try again";
                }
            }
            return mr;
        }

        public MethodReturn ChangePassword()
        {
            MethodReturn mr = ValidateInput();
            return mr;
        }

        public MethodReturn LiftUser()
        {
            MethodReturn mr = ValidateInput();
            return mr;
        }
        public MethodReturn DeleteUser()
        {
            MethodReturn mr = ValidateInput();
            if (mr.IsSuccess)
            {
                if (UserExist(Username))
                {
                    mr.IsSuccess = dbcon.Exec("UPDATE tbl_users SET isdeleted=1 WHERE uid='" + UID + "'");
                    mr.Message = mr.IsSuccess ? "Successfully archived User." : "Something went wrong pleas try again.";
                }
                else
                {
                    mr.Message = "User does not exist!";
                }
            }
            else
            {
                mr.Message = "Invalid inputs!";
            }
            return mr;
        }
        #endregion
    }

    public enum UserAccessType
    {
        Admin,
        Teacher,
        Student,
        Registrar,
        Accounting,
        Invalid
    }
}
