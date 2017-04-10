/* FileName: EmailService.cs
Project Name: MvcSendMail
Date Created: 9/15/2014PM
Description: Auto-generated
Version: 1.0.0.0
Author:	Lê Thanh Tuấn - Khoa CNTT
Author Email: tuanitpro@gmail.com
Author Mobile: 0976060432
Author URI: http://tuanitpro.com
License: 

*/

using System.Net;
using System.Net.Mail;
using System.Text;

namespace ParaQum.Models
{
    /// <summary>
    /// Email Service
    /// Class gửi mail trong MVC
    /// </summary>
    public interface EmailService
    {
        /// <summary>
        /// Hàm thực thi gửi email. 
        /// </summary>
        /// <param name="smtpUserName">Tên đăng nhập email gửi thư: vd:tuanitpro</param>
        /// <param name="smtpPassword">Mật khẩu của email gửi thư</param>
        /// <param name="smtpHost">Host email. vd smtp.gmail.com</param>
        /// <param name="smtpPort">Port vd: 465</param>
        /// <param name="toEmail">Email nhận vd: tuanitpro@gmail.com</param>
        /// <param name="subject">Chủ đề</param>
        /// <param name="body">Nội dung thư gửi</param>
        /// <returns>True-Thành công/False-Thất bại</returns>
         bool Send(string smtpUserName, string smtpPassword, string smtpHost, int smtpPort,
            string toEmail, string subject, string body);
        
    }
}