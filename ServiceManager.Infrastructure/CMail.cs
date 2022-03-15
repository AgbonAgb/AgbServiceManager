using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManager.Infrastructure
{
    public class CMail
    {
        //private ArrayList ToEmail = new ArrayList();
        //public string DisplayName{ get; set; }
        ///// <summary>
        ///// one or more destinations or recipient emaill address
        ///// </summary>
        //public ArrayList ToEmail { get; set; }
        ///// <summary>
        ///// The email address that is originating this message
        ///// </summary>
        //public ArrayList FromEmail { get; set; }
        ///// <summary>
        ///// an one or more email address where default reply goes to
        ///// </summary>
        //public ArrayList ReplyTo { get; set; }
        ///// <summary>
        ///// an one or more email address to be copied
        ///// </summary>
        //public ArrayList CCEmail { get; set; }
        ///// <summary>
        ///// an one or more email address to be back copied
        ///// </summary>
        //public ArrayList BCCEmail { get; set; }
        ///// <summary>
        ///// The subject
        ///// </summary>
        //public string Subject { get; set; }
        ///// <summary>
        ///// The body of the message in text format
        ///// </summary>
        //public string Body { get; set; }
        ///// <summary>
        ///// The datetime stamp when the package was put together
        ///// </summary>
        //public string ComposedDate { get; set; }
        ///// <summary>
        ///// The is the name of the application where this emails message originates from
        ///// </summary>
        //public string SourceApplication { get; set; }
        ////m_AttachedFile
        //public string AttachedFile { get; set; }

        private string m_DisplayName;
        private ArrayList m_ToEmail = new ArrayList();
        private ArrayList m_FromEmail = new ArrayList();
        private ArrayList m_ReplyTo = new ArrayList();
        private ArrayList m_CCEmail = new ArrayList();
        private ArrayList m_BCCEmail = new ArrayList();
        private string m_Subject;
        private string m_Body;
        private string m_ComposedDate;
        private string m_SourceApplication;
        private string m_AttachedFile;

        /// <summary>
        /// This is the display name in the owner column (a caption for the FromEmail)
        /// </summary>
        public string DisplayName
        {
            get { return m_DisplayName; }
            set { m_DisplayName = value; }

        }
        /// <summary>
        /// one or more destinations or recipient emaill address
        /// </summary>
        public ArrayList ToEmail
        {
            get { return m_ToEmail; }
            set { m_ToEmail = value; }
        }
        /// <summary>
        /// The email address that is originating this message
        /// </summary>
        public ArrayList FromEmail
        {
            get { return m_FromEmail; }
            set { m_FromEmail = value; }

        }
        /// <summary>
        /// an one or more email address where default reply goes to
        /// </summary>
        public ArrayList ReplyTo
        {
            get { return m_ReplyTo; }
            set { m_ReplyTo = value; }
        }
        /// <summary>
        /// an one or more email address to be copied
        /// </summary>
        public ArrayList CCEmail
        {
            get { return m_CCEmail; }
            set { m_CCEmail = value; }

        }
        /// <summary>
        /// an one or more email address to be back copied
        /// </summary>
        public ArrayList BCCEmail
        {
            get { return m_BCCEmail; }
            set { m_BCCEmail = value; }
        }
        /// <summary>
        /// The subject
        /// </summary>
        public string Subject
        {
            get { return m_Subject; }
            set { m_Subject = value; }

        }
        /// <summary>
        /// The body of the message in text format
        /// </summary>
        public string Body
        {
            get { return m_Body; }
            set { m_Body = value; }
        }
        /// <summary>
        /// The datetime stamp when the package was put together
        /// </summary>
        public string ComposedDate
        {
            get { return m_ComposedDate; }
            set { m_ComposedDate = value; }

        }
        /// <summary>
        /// The is the name of the application where this emails message originates from
        /// </summary>
        public string SourceApplication
        {
            get { return m_SourceApplication; }
            set { m_SourceApplication = value; }

        }
        //m_AttachedFile
        public string AttachedFile
        {
            get { return m_AttachedFile; }
            set { m_AttachedFile = value; }

        }
    }
}
