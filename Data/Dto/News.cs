using Sharp_231.Attributes;

namespace Sharp_231.Data.Dto;

[TableName("News")]
public class News
{
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime Moment { get; set; }

    public override string ToString() => 
        $"{Moment:dd.MM} {Title} <|> {Content} ({Id.ToString()[..3]}...)";
}