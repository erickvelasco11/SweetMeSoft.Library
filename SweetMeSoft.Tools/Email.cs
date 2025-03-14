﻿using SweetMeSoft.Base.Tools;

using System.Net;
using System.Net.Mail;

namespace SweetMeSoft.Tools;

public class Email
{
    public static void Send(EmailOptions options)
    {
        if (string.IsNullOrEmpty(EmailOptions.Sender))
        {
            throw new ArgumentException("The sender must be initialized with EmailOptions.Sender");
        }

        if (string.IsNullOrEmpty(EmailOptions.Password))
        {
            throw new ArgumentException("The password must be initialized with EmailOptions.Password");
        }

        var port = options.Host == EmailHost.Gmail ? 587
            : options.Host == EmailHost.Outlook ? 25
            : options.Host == EmailHost.Webmail ? 587
            : 0;
        var host = options.Host == EmailHost.Gmail ? "smtp.gmail.com"
            : options.Host == EmailHost.Outlook ? "smtp.live.com"
            : options.Host == EmailHost.Webmail ? "smtpout.secureserver.net"
            : "";
        var ssl = options.Host is EmailHost.Gmail or EmailHost.Outlook or EmailHost.Webmail;

        try
        {
            var client = new SmtpClient
            {
                EnableSsl = ssl,
                UseDefaultCredentials = false,
                Port = port,
                Host = host,
                Credentials = new NetworkCredential(EmailOptions.Sender, EmailOptions.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            var message = new MailMessage()
            {
                From = new MailAddress(EmailOptions.Sender),
                Subject = options.Subject,
                IsBodyHtml = true,
                Body = options.HtmlBody,
            };

            message.To.Add(new MailAddress(options.Destinatary));
            foreach (var additionalDestinatary in options.AdditionalDestinataries)
            {
                message.To.Add(new MailAddress(additionalDestinatary));
            }

            foreach (var cc in options.CC)
            {
                message.CC.Add(new MailAddress(cc));
            }

            foreach (var cco in options.CCO)
            {
                message.Bcc.Add(new MailAddress(cco));
            }

            foreach (var file in options.Attachments)
            {
                file.Stream.Stream.Position = 0;
                if (file.IsLinked)
                {
                    var linkedResource = new LinkedResource(file.Stream.Stream, file.Stream.GetContentType())
                    {
                        ContentId = file.Cdi
                    };

                    var alternateView = AlternateView.CreateAlternateViewFromString(options.HtmlBody, null, "text/html");
                    alternateView.LinkedResources.Add(linkedResource);
                    message.AlternateViews.Add(alternateView);
                }
                else
                {
                    message.Attachments.Add(new Attachment(file.Stream.Stream, file.Stream.FileName, file.Stream.GetContentType()));
                }
            }

            client.Send(message);
        }
        catch (Exception)
        {
            throw;
        }
    }
}