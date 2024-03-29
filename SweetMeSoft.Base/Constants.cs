﻿namespace SweetMeSoft.Base;

public class Constants
{

    public static readonly string[] Months =
    [
        "January",
        "February",
        "March",
        "April",
        "May",
        "June",
        "July",
        "August",
        "September",
        "October",
        "November",
        "December"
    ];

    public enum ContentType
    {
        aac,
        bin,
        csv,
        dat,
        doc,
        docx,
        gif,
        jpg,
        mov,
        mp3,
        mp4,
        msg,
        pagos,
        pdf,
        png,
        ppt,
        pptx,
        rar,
        txt,
        wav,
        xlsx,
        xls,
        xml,
        zip
    }

    public static readonly Dictionary<ContentType, string> ContentTypesDict = new()
    {
        { ContentType.aac, "audio/x-aac" },
        { ContentType.bin, "application/octet-stream" },
        { ContentType.csv, "text/csv" },
        { ContentType.doc, "application/msword" },
        { ContentType.docx, "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
        { ContentType.dat, "text/plain" },
        { ContentType.gif, "image/gif" },
        { ContentType.jpg, "image/jpeg" },
        { ContentType.mov, "video/quicktime" },
        { ContentType.mp3, "audio/mpeg" },
        { ContentType.mp4, "video/mp4" },
        { ContentType.msg, "application/vnd.ms-outlook" },
        { ContentType.pagos, "text/plain" },
        { ContentType.png, "image/png" },
        { ContentType.pdf, "application/pdf" },
        { ContentType.ppt, "application/vnd.ms-powerpoint" },
        { ContentType.pptx, "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
        { ContentType.rar, "application/vnd.rar" },
        { ContentType.txt, "text/plain" },
        { ContentType.wav, "audio/wav" },
        { ContentType.xls, "application/vnd.ms-excel" },
        { ContentType.xlsx, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
        { ContentType.xml, "text/xml" },
        { ContentType.zip, "application/zip" },
    };

    public enum ConfirmationTypes
    {
        Email,
        ResetPassword
    }

    public const string KEY_JWT_TOKEN = "KEY_JWT_TOKEN";
    public const string KEY_CURRENT_USER = "CURRENT_USER";
    public const string KEY_CURRENT_USER_ID = "CURRENT_USER_ID";
    public const string KEY_CURRENT_USER_TYPE = "CURRENT_USER_TYPE";
    public const string KEY_CURRENT_PASSWORD = "CURRENT_PASSWORD";
    public const string KEY_IS_USER_COMPLETE = "IS_USER_COMPLETE";
    public const string KEY_NOTIFICATIONS_TOKEN = "KEY_NOTIFICATIONS_TOKEN";

    public static string API_URL = "";


    public static ContentType GetContentType(string extension)
    {
        extension = extension.ToLower();
        var key = ContentTypesDict.FirstOrDefault(model => model.Key.ToString().ToLower() == extension);
        return key.Value == null ? ContentType.txt : key.Key;
    }
}