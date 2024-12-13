using Entities.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Shared.DataTransferObjects;
using System.Text;

namespace UTaskManager;

public class CvsOutputFormatter : TextOutputFormatter
{
    public CvsOutputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    protected override bool CanWriteType(Type? type)
    {
        if (type is null)
        {
            return false;
        }
        return true;
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        var response = context.HttpContext.Response;
        var buffer = new StringBuilder();

        if (context.Object is IEnumerable<object> objects)
        {
            var type = objects.GetType().GenericTypeArguments[0];
            foreach (var obj in (IEnumerable<object>)context.Object)
            {
                FormatCsv(buffer, obj);
            }
        }
        else
        {
            FormatCsv(buffer, context.Object);
        }

        await response.WriteAsync(buffer.ToString());
    }

    private static void FormatCsv(StringBuilder buffer, object obj)
    {
        var properties = obj.GetType().GetProperties();
        var values = properties.Select(p =>
        {
            var value = p.GetValue(obj);
            return value is string ? $"\"{value}\"" : value?.ToString();
        });
        var csvLine = string.Join(",", values);
        buffer.AppendLine(csvLine);
    }
}
