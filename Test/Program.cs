// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using NapCatScript.Core.JsonFormat;

string json = """
              """;
var jsonObject = JsonSerializer.Deserialize<ArrayMsg>(json);
Console.WriteLine(jsonObject);