using MimeKit.Text;

namespace BackPanel.SMTP.Interfaces;

public interface ISmtpService
{
    Task SendMessageAsync(string from, string to, string subject, string content, TextFormat format);
}