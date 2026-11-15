using PitWallSecurity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Security;
using System.Xml.Linq;

namespace PitWallTryIt.Security
{
    public static class AccountStore
    {

        //stores usernames, passwords and checks when users log in 
        private static readonly object FileLock = new object(); // one writer at a time

        private static string MemberPath { get { return HostingEnvironment.MapPath("~/App_Data/Member.xml"); } }
        private static string StaffPath { get { return HostingEnvironment.MapPath("~/App_Data/Staff.xml"); } }

        //XML files if missing for staff and members 
        public static void EnsureFiles()
        {

            lock (FileLock)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(MemberPath));
                if (!File.Exists(MemberPath)) new XDocument(new XElement("Members")).Save(MemberPath);
                if (!File.Exists(StaffPath)) new XDocument(new XElement("Staff")).Save(StaffPath);

            }
            if (!UserExists(StaffPath, "Staff", "TA")) AddUser(StaffPath, "Staff", "TA", "Cse445!");

        }

        //initialize the validation process 
        public static bool ValidateMember(string user, string pass) { return Validate(MemberPath, "Member", user, pass); }
        public static bool ValidateStaff(string user, string pass) { return Validate(StaffPath, "Staff", user, pass); }
        public static bool MemberExists(string user) { return UserExists(MemberPath, "Member", user); }
        public static bool AddMember(string user, string pass) { return AddUser(MemberPath, "Member", user, pass); }
        public static bool AddStaff(string user, string pass) { return AddUser(StaffPath, "Staff", user, pass); }

        //password change for members 
        public static bool ChangeMemberPassword(string user, string oldPass, string newPass)
        {
            if (string.IsNullOrEmpty(newPass)) return false;
            lock (FileLock)
            {
                XDocument doc = XDocument.Load(MemberPath);
                XElement u = Find(doc, "Member", user);
                if (u == null || (string)u.Element("Password") != PitWallCrypto.Encrypt(oldPass ?? "")) return false;
                u.SetElementValue("Password", PitWallCrypto.Encrypt(newPass));
                doc.Save(MemberPath);
                return true;
            }
        }

        private static bool Validate(string path, string tag, string user, string pass)
        {
            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrEmpty(pass)) return false;
            lock (FileLock)
            {
                XElement u = Find(XDocument.Load(path), tag, user);
                return u != null && (string)u.Element("Password") == PitWallCrypto.Encrypt(pass);
            }
        }

        private static bool UserExists(string path, string tag, string user)
        {
            if (string.IsNullOrWhiteSpace(user)) return false;
            lock (FileLock) { return Find(XDocument.Load(path), tag, user) != null; }
        }


        //adds user to the account
        private static bool AddUser(string path, string tag, string user, string pass)
        {
            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrEmpty(pass)) return false;
            lock (FileLock)
            {
                XDocument doc = XDocument.Load(path);
                if (Find(doc, tag, user) != null) return false;
                doc.Root.Add(new XElement(tag,
                    new XElement("Username", user.Trim()),
                    new XElement("Password", PitWallCrypto.Encrypt(pass)),
                    new XElement("Created", DateTime.Now.ToString("yyyy-MM-dd"))));
                doc.Save(path);
                return true;
            }
        }

        private static XElement Find(XDocument doc, string tag, string user)
        {
            string name = user.Trim();
            return doc.Root.Elements(tag).FirstOrDefault(x =>
                string.Equals((string)x.Element("Username"), name, StringComparison.OrdinalIgnoreCase));
        }

    }

    // By: Ayomide Oshilaja. Used by Login and Izzy's Create account page section
    public static class AuthHelper
    {

        public static void SignIn(string username, string role, bool persistant)
        {
            DateTime expires = persistant ? DateTime.Now.AddDays(7) : DateTime.Now.AddMinutes(30);
            var ticket = new FormsAuthenticationTicket(1, username, DateTime.Now, expires, persistant, role, FormsAuthentication.FormsCookiePath);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket)) {
                HttpOnly = true, 
                Path = FormsAuthentication.FormsCookiePath
            };

            if (persistant) cookie.Expires = expires;
            HttpContext.Current.Response.Cookies.Add(cookie);

        }

    }

}