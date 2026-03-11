public static class AppConstants
{
    public const string IndividualUser = "Individual";
    public const string OrganizationalUser = "Organization";
    private const string clientBaseUrl = "http://localhost:4200";
    public const string EmailSubject = "Welcome to AuraDocs. Please reset password.";
    public const string HtmlBody = @"
                                    <!DOCTYPE html>
                                    <html>
                                    <head>
                                    <style>
                                        body {{ font-family: Arial; }}
                                        .btn {{
                                            color: #4CAF50;
                                            padding: 10px 16px;
                                            text-decoration: none;
                                            border-radius: 4px;
                                        }}
                                    </style>
                                    </head>
                                    <body>
                                    <h2>Welcome to AuraDocs</h2>
                                    <p>Your account has been created successfully.</p>
                                    <a class='btn' href='{0}'>Click here</a> to verify auradocs account.
                                    </body>
                                    </html>";

    public const string resetPasswordVerificationUrl = $"http://localhost:5170/User/verify-token?token={{0}}";
    public const string  verifyResultSuccessUrl = $"{clientBaseUrl}/reset-password?token={{0}}";

    public const string verifyResultFailedUrl = $"{clientBaseUrl}/verification-failed?status=failed";
    public const string existingTitlePostfix = "(1)";
    public const string duplicateDocumentTitlePostFix = "(copy)";
    public const string Auradocs_access_token_key = "auradocs_access_token";

}