using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Mails.Templates
{
    internal class EmailConfirmationTemplate
    {
        public const string Value = @"<!DOCTYPE html>
        <html lang=""en"">
          <head>
            <meta charset=""UTF-8"" />
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
            <title>Email Confirmation</title>
            <style>
              /* Reset CSS */
              body {
                font-family: ""Segoe UI"", Tahoma, Geneva, Verdana, sans-serif;
                line-height: 1.6;
                background-color: #f9f9f9;
                margin: 0;
                padding: 0;
              }
              .container {
                max-width: 600px;
                margin: 20px auto;
                padding: 20px;
                background-color: #fff;
                border-radius: 8px;
                box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
              }
              h1 {
                color: #0056b3;
                margin-bottom: 20px;
                font-size: 24px;
              }
              p {
                margin-bottom: 10px;
                color: #444;
              }
              strong {
                font-weight: bold;
                color: #007bff;
              }
              .button {
                display: inline-block;
                padding: 12px 24px;
                background-color: #007bff;
                color: #fff;
                text-decoration: none;
                border-radius: 5px;
                transition: background-color 0.3s ease;
              }
              .button:hover {
                background-color: #0056b3;
              }
            </style>
          </head>
          <body>
            <div class=""container"">
              <h1>Email Confirmation</h1>
              <p>Dear [UserName],</p>
              <p>
                Thank you for your action. Please confirm your email by clicking the
                button below:
              </p>
              <a href=""[ConfirmURL]"" target=""_blank"" class=""button"">Confirm Email</a>
              <p>If you did not perform this action, you can ignore this email.</p>
              <p>Thank you,<br />EDUWAY Team</p>
            </div>
          </body>
        </html>
        ";
    }
}
