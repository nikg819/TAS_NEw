namespace TAS_Test.Models;

public class Article
{
    public int ArticleDatabaseId { get; set; }
    public int? ArticleNumber { get; set; }
    public string ArticleName { get; set; }
    public string ArticleDescription { get; set; }
    public string ArticlePrice { get; set; }

    public Article (){}
    
    public Article(int ArticleDatabaseId, int ArticleNumber, string ArticleName, string ArticleDescription, string ArticlePrice)
    {
        this.ArticleDatabaseId = ArticleDatabaseId;
        this.ArticleNumber = ArticleNumber;
        this.ArticleName = ArticleName;
        this.ArticleDescription = ArticleDescription;
        this.ArticlePrice = ArticlePrice;
    }
    
}