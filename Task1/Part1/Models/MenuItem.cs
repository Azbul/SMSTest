namespace Part1.Models;

public sealed record MenuItem(
    string Id,
    string Article,
    string Name,
    decimal? Price,
    bool IsWeighted,
    string FullPath,
    IEnumerable<string> Barcodes);