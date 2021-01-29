using UnityEngine;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;

public class EmailFactory : MonoBehaviour
{
    public static EmailFactory instance;

    public Player player => Player.instance;

    private void Start()
    {
        instance = this;
    }

    public void SendEmail(List<string> results)
    {
        MailMessage mail = new MailMessage();
        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
        SmtpServer.Timeout = 10000;
        SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
        SmtpServer.UseDefaultCredentials = false;
        SmtpServer.Port = 587;

        mail.From = new MailAddress("gamelabkit@gamelaboost.nl");
        mail.To.Add(new MailAddress("gamelabkit@gamelaboost.nl"));

        mail.Subject = $"Voorkeur resultaat van: {player.PlayerName}";
        mail.Body = GetBody(results);


        SmtpServer.Credentials = new NetworkCredential("gamelabkit@gamelaboost.nl", "w8wOORD!1") as ICredentialsByHost; SmtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        };

        mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
        SmtpServer.Send(mail);
    }

    private string GetBody(List<string> results)
    {
        string outcome = $"Dit zijn de voorkeuren van {player.PlayerName}\n";

        outcome += $"Medium: {results[0]}\n";
        outcome += $"Artstyle: {results[1]}\n";
        outcome += $"Doel: {results[2]}\n";
        outcome += $"Mechanics: {results[3]}\n";
        outcome += $"Genre: {results[4]}\n";

        return outcome;
    }
}