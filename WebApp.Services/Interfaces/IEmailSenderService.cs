﻿namespace WebApp.Services.Interfaces;

public interface IEmailSenderService
{
    Task SendEmailConfirmationAsync(string email, string confirmationUri);

    Task SendChangePasswordAsync(string email, string changePasswordUri);
}
