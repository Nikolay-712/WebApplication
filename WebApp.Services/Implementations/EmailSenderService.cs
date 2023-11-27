﻿using SendGrid.Helpers.Mail;
using SendGrid;
using WebApp.Services.Interfaces;
using Microsoft.Extensions.Options;
using WebApp.Common.Configurations;
using Microsoft.Extensions.Logging;

namespace WebApp.Services.Implementations;

public class EmailSenderService : IEmailSenderService
{
    private readonly EmailSenderSettings _senderSettings;
    private readonly ILogger<EmailSenderService> _logger;

    public EmailSenderService(IOptions<EmailSenderSettings> options, ILogger<EmailSenderService> logger)
    {
        _senderSettings = options.Value;
        _logger = logger;
    }

    public async Task SendEmailConfirmationAsync(string email, string confirmationUri)
    {
        SendGridClient client = new(_senderSettings.Key);
        EmailAddress from = new(_senderSettings.Sender, _senderSettings.Name);

        string subject = "Sending with SendGrid is Fun";
        EmailAddress to = new EmailAddress(email, "Example User");

        string htmlContent = $"<html>\r\n<head>\r\n  <meta charset=\"utf-8\">\r\n  <meta http-equiv=\"x-ua-compatible\" content=\"ie=edge\">\r\n  <title>Email Confirmation</title>\r\n  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n  <style type=\"text/css\">\r\n    @media screen {{\r\n      @font-face {{\r\n        font-family: 'Source Sans Pro';\r\n        font-style: normal;\r\n        font-weight: 400;\r\n        src: local('Source Sans Pro Regular'), local('SourceSansPro-Regular'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/ODelI1aHBYDBqgeIAH2zlBM0YzuT7MdOe03otPbuUS0.woff) format('woff');\r\n      }}\r\n\r\n      @font-face {{\r\n        font-family: 'Source Sans Pro';\r\n        font-style: normal;\r\n        font-weight: 700;\r\n        src: local('Source Sans Pro Bold'), local('SourceSansPro-Bold'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/toadOcfmlt9b38dHJxOBGFkQc6VGVFSmCnC_l7QZG60.woff) format('woff');\r\n      }}\r\n    }}\r\n\r\n   \r\n    body,\r\n    table,\r\n    td,\r\n    a {{\r\n      -ms-text-size-adjust: 100%;\r\n      -webkit-text-size-adjust: 100%;\r\n    }}\r\n\r\n    \r\n    table,\r\n    td {{\r\n      mso-table-rspace: 0pt;\r\n      mso-table-lspace: 0pt;\r\n    }}\r\n    img {{\r\n      -ms-interpolation-mode: bicubic;\r\n    }}\r\n    a[x-apple-data-detectors] {{\r\n      font-family: inherit !important;\r\n      font-size: inherit !important;\r\n      font-weight: inherit !important;\r\n      line-height: inherit !important;\r\n      color: inherit !important;\r\n      text-decoration: none !important;\r\n    }}\r\n    div[style*=\"margin: 16px 0;\"] {{\r\n      margin: 0 !important;\r\n    }}\r\n    body {{\r\n      width: 100% !important;\r\n      height: 100% !important;\r\n      padding: 0 !important;\r\n      margin: 0 !important;\r\n    }}\r\n    table {{\r\n      border-collapse: collapse !important;\r\n    }}\r\n\r\n    a {{\r\n      color: #1a82e2;\r\n    }}\r\n\r\n    img {{\r\n      height: auto;\r\n      line-height: 100%;\r\n      text-decoration: none;\r\n      border: 0;\r\n      outline: none;\r\n    }}\r\n  </style>\r\n</head>\r\n\r\n<body style=\"background-color: #e9ecef;\">\r\n  <div class=\"preheader\" style=\"display: none; max-width: 0; max-height: 0; overflow: hidden; font-size: 1px; line-height: 1px; color: #fff; opacity: 0;\">\r\n    A preheader is the short summary text that follows the subject line when an email is viewed in the inbox.\r\n  </div>\r\n  <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n    <tr>\r\n      <td align=\"center\" bgcolor=\"#e9ecef\">\r\n        <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n          <tr>\r\n            <td align=\"left\" bgcolor=\"#ffffff\" style=\"padding: 36px 24px 0; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; border-top: 3px solid #d4dadf;\">\r\n              <h1 style=\"margin: 0; font-size: 32px; font-weight: 700; letter-spacing: -1px; line-height: 48px;\">Confirm Your Email Address</h1>\r\n            </td>\r\n          </tr>\r\n        </table>\r\n      </td>\r\n    </tr>\r\n    <tr>\r\n      <td align=\"center\" bgcolor=\"#e9ecef\">\r\n        <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n\r\n          \r\n          <tr>\r\n            <td align=\"left\" bgcolor=\"#ffffff\" style=\"padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;\">\r\n              <p style=\"margin: 0;\">Tap the button below to confirm your email address. If you didn't create an account with <a href=\"https://blogdesire.com\">Paste</a>, you can safely delete this email.</p>\r\n            </td>\r\n          </tr>\r\n          <tr>\r\n            <td align=\"left\" bgcolor=\"#ffffff\">\r\n              <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n                <tr>\r\n                  <td align=\"center\" bgcolor=\"#ffffff\" style=\"padding: 12px;\">\r\n                    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\r\n                      <tr>\r\n                        <td align=\"center\" bgcolor=\"#1a82e2\" style=\"border-radius: 6px;\">\r\n                          <a href=\"{confirmationUri}\" target=\"_blank\" style=\"display: inline-block; padding: 16px 36px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; color: #ffffff; text-decoration: none; border-radius: 6px;\">Click here </a>\r\n                        </td>\r\n                      </tr>\r\n                    </table>\r\n                  </td>\r\n                </tr>\r\n              </table>\r\n            </td>\r\n          </tr>\r\n        \r\n          <tr>\r\n   \r\n          </tr>\r\n         \r\n        </table>\r\n        \r\n      </td>\r\n    </tr>\r\n    \r\n    <tr>\r\n      <td align=\"center\" bgcolor=\"#e9ecef\" style=\"padding: 24px;\">\r\n        \r\n        <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n          <tr>\r\n            <td align=\"center\" bgcolor=\"#e9ecef\" style=\"padding: 12px 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 14px; line-height: 20px; color: #666;\">\r\n              <p style=\"margin: 0;\">You received this email because we received a request for [type_of_action] for your account. If you didn't request [type_of_action] you can safely delete this email.</p>\r\n            </td>\r\n          </tr>\r\n          <tr>\r\n            <td align=\"center\" bgcolor=\"#e9ecef\" style=\"padding: 12px 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 14px; line-height: 20px; color: #666;\">\r\n              <p style=\"margin: 0;\">To stop receiving these emails, you can <a href=\"https://www.blogdesire.com\" target=\"_blank\">unsubscribe</a> at any time.</p>\r\n              <p style=\"margin: 0;\">Paste 1234 S. Broadway St. City, State 12345</p>\r\n            </td>\r\n          </tr>\r\n        </table>\r\n      </td>\r\n    </tr>\r\n  </table>\r\n \r\n</body>\r\n\r\n</html>";
        SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
        Response response = await client.SendEmailAsync(msg);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Email sender response is not success status code {statusCode}",response.StatusCode);
           
        }
    }
}