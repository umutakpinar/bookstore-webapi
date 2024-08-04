using WebAPI.Utilities.Formatters;

namespace WebAPI.Extensions;

public static class IMvcBuilderExtensions
{
    public static IMvcBuilder AddCustomCsvOutputFormatter(this IMvcBuilder builder) =>
        builder.AddMvcOptions(config => config.OutputFormatters.Add(new CsvOutputFormatter()));
}