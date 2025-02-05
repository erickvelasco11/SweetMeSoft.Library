﻿using SweetMeSoft.Base.Captcha;
using SweetMeSoft.Base.Tools;
using SweetMeSoft.Tools;

using TwoCaptcha.Captcha;

namespace SweetMeSoft.Captcha;

public class Solver
{
    private static TwoCaptcha.Captcha.Captcha SolveNormal(CaptchaOptions options)
    {
        var captcha = new Normal();
        captcha.SetBase64(options.ImageBase64);
        captcha.SetCaseSensitive(true);
        captcha.SetHintText(options.HintText);
        return captcha;
    }

    private static TwoCaptcha.Captcha.Captcha SolveReCaptchaV2(CaptchaOptions options)
    {
        var captcha = new ReCaptcha();
        captcha.SetSiteKey(options.SiteKey);
        captcha.SetUrl(options.SiteUrl);
        return captcha;
    }

    private static TwoCaptcha.Captcha.Captcha SolveReCaptchaV2Invisible(CaptchaOptions options)
    {
        var captcha = new ReCaptcha();
        captcha.SetSiteKey(options.SiteKey);
        captcha.SetUrl(options.SiteUrl);
        captcha.SetInvisible(true);
        captcha.SetAction("verify");
        return captcha;
    }

    private static TwoCaptcha.Captcha.Captcha SolveReCaptchaV3(CaptchaOptions options)
    {
        var captcha = new ReCaptcha();
        captcha.SetSiteKey(options.SiteKey);
        captcha.SetUrl(options.SiteUrl);
        captcha.SetVersion("v3");
        captcha.SetDomain("google.com");
        captcha.SetAction("verify");
        captcha.SetScore(0.3);
        return captcha;
    }

    private static TwoCaptcha.Captcha.Captcha SolveReCaptchaEnterprise(CaptchaOptions options)
    {
        var captcha = new ReCaptcha();
        captcha.SetSiteKey(options.SiteKey);
        captcha.SetUrl(options.SiteUrl);
        captcha.SetInvisible(true);
        captcha.SetEnterprise(true);
        return captcha;
    }

    public static async Task<string> SolveAsync(CaptchaOptions options)
    {
        Console.WriteLine("Resolving Captcha...");
        var solver = new TwoCaptcha.TwoCaptcha(options.TwoCaptchaKey)
        {
            DefaultTimeout = 120,
            RecaptchaTimeout = 600,
            PollingInterval = 10
        };

        TwoCaptcha.Captcha.Captcha captcha;
        switch (options.CaptchaType)
        {
            case CaptchaType.Normal:
                captcha = SolveNormal(options);
                break;

            case CaptchaType.ReCaptchaV2:
                captcha = SolveReCaptchaV2(options);
                break;

            case CaptchaType.ReCaptchaV2Invisible:
                captcha = SolveReCaptchaV2Invisible(options);
                break;

            case CaptchaType.ReCaptchaV3:
                captcha = SolveReCaptchaV3(options);
                break;

            case CaptchaType.ReCaptchaEnterprise:
                captcha = SolveReCaptchaEnterprise(options);
                break;

            default:
                return "Captcha " + options.CaptchaType + " it's not implemented yet.";
        }

        var code = "";
        try
        {
            var balance = await solver.Balance();
            Console.WriteLine("Balance: " + balance.ToString("#.00"));
            if (balance < 1 && !string.IsNullOrEmpty(options.EmailNotifications))
            {
                Email.Send(new EmailOptions(options.EmailNotifications)
                {
                    Subject = "Captcha solver Funds",
                    HtmlBody = "The balance of your Captcha Solver is under 1 USD. Please recharge your account."
                });
            }

            var captchaId = await solver.Send(captcha);
            while (code is "" or null)
            {
                Thread.Sleep(2000);
                code = await solver.GetResult(captchaId);
            }

            Console.WriteLine(code);
            return code;
        }
        catch (Exception e)
        {
            Console.Error.WriteLine("Error occurred: " + e.Message);
        }

        return "";
    }
}