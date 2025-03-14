﻿using CsvHelper;
using CsvHelper.Configuration;

using SweetMeSoft.Base;

using System.Globalization;

namespace SweetMeSoft.Files;

public class Csv
{
    public static async Task<IEnumerable<T>> Read<T>(StreamFile streamFile, bool hasHeader = true, string delimiter = "|", string decimalSeparator = ".")
    {
        return await Read<T>(streamFile.Stream, hasHeader, delimiter, decimalSeparator);
    }

    public static async Task<IEnumerable<T>> Read<T>(Stream stream, bool hasHeader = true, string delimiter = "|", string decimalSeparator = ".")
    {
        CultureInfo culture = (!(decimalSeparator == ".")) ? CultureInfo.GetCultureInfo("es-CO") : CultureInfo.GetCultureInfo("en-US");

        var config = new CsvConfiguration(culture)
        {
            Delimiter = delimiter,
            IgnoreBlankLines = true,
            HasHeaderRecord = hasHeader,
            BadDataFound = null,
            MissingFieldFound = null,
            ReadingExceptionOccurred = (ReadingExceptionOccurredArgs ex) => false
        };

        var copiedStream = new MemoryStream();
        stream.Position = 0;
        await stream.CopyToAsync(copiedStream);
        copiedStream.Position = 0;

        using var reader = new StreamReader(copiedStream);
        using var csv = new CsvReader(reader, config);
        return await csv.GetRecordsAsync<T>().ToListAsync();
    }

    public static async Task<IEnumerable<T>> Read<T, TMap>(StreamFile streamFile, bool hasHeader = true, string delimiter = "|", string decimalSeparator = ".") where TMap : ClassMap
    {
        return await Read<T, TMap>(streamFile.Stream, hasHeader, delimiter, decimalSeparator);
    }

    public static async Task<IEnumerable<T>> Read<T, TMap>(Stream stream, bool hasHeader = true, string delimiter = "|", string decimalSeparator = ".") where TMap : ClassMap
    {
        CultureInfo culture = (!(decimalSeparator == ".")) ? CultureInfo.GetCultureInfo("es-CO") : CultureInfo.GetCultureInfo("en-US");

        var config = new CsvConfiguration(culture)
        {
            Delimiter = delimiter,
            IgnoreBlankLines = true,
            HasHeaderRecord = hasHeader,
            BadDataFound = null,
            MissingFieldFound = null,
            ReadingExceptionOccurred = (ex) =>
            {
                return false;
            }
        };

        var copiedStream = new MemoryStream();
        stream.Position = 0;
        await stream.CopyToAsync(copiedStream);
        copiedStream.Position = 0;

        using var reader = new StreamReader(copiedStream);
        using var csv = new CsvReader(reader, config);
        csv.Context.RegisterClassMap<TMap>();
        return await csv.GetRecordsAsync<T>().ToListAsync();
    }

    public static async Task<StreamFile> Create<T>(IEnumerable<T> list, string fileName = "", string delimiter = ",", bool hasHeader = true)
    {
        var stream = new MemoryStream();
        var config = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = delimiter, HasHeaderRecord = hasHeader };
        using var writer = new StreamWriter(stream);
        using var csv = new CsvWriter(writer, config);
        await csv.WriteRecordsAsync(list);
        await writer.FlushAsync();
        var copiedStream = new MemoryStream();
        stream.Position = 0L;
        await stream.CopyToAsync(copiedStream);
        copiedStream.Position = 0L;

        fileName = string.IsNullOrEmpty(fileName) ? Guid.NewGuid().ToString("N") : fileName;
        return new StreamFile(fileName, copiedStream, Constants.ContentType.csv);
    }
}