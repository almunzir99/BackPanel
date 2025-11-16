using BackPanel.SMTP.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace BackPanel.SMTP.Services;

public class SmtpService : ISmtpService
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string password { get; set; }
    public SmtpService(string host, int port, string username, string password)
    {
        Host = host;
        Port = port;
        Username = username;
        this.password = password;
    }
    public async Task SendMessageAsync(string from, string to, string subject, string content, TextFormat format)
    {
        using var client = new SmtpClient();
        await client.ConnectAsync(Host, Port);
        await client.AuthenticateAsync(Username, password);
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(from));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart(format) { Text = content };
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}