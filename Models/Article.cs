using ReactiveUI;

namespace TAS_Test.Models;

public class Article :ReactiveObject
{
    public int ArticleDatabaseId { get; set; }
    public string ArticleNumber { get; set; }
    public string ArticleName { get; set; }
    public string ArticleDescription { get; set; }
    public string ArticlePrice { get; set; }
    
    private bool _isChecked;
    public bool IsChecked
    {
        get => _isChecked;
        set => this.RaiseAndSetIfChanged(ref _isChecked, value);
    }

    public Article (){}
    
    public Article(int ArticleDatabaseId, string ArticleNumber, string ArticleName, string ArticleDescription, string ArticlePrice)
    {
        this.ArticleDatabaseId = ArticleDatabaseId;
        this.ArticleNumber = ArticleNumber;
        this.ArticleName = ArticleName;
        this.ArticleDescription = ArticleDescription;
        this.ArticlePrice = ArticlePrice;
    }
    
}