using System;
using System.Reactive;
using ReactiveUI;
using TAS_Test.Models;

namespace TAS_Test.ViewModels;

public class NewArticleViewModel : ReactiveObject
{
    public Action<ReactiveObject>? Navigate { get; set; }
    public ReactiveCommand<Unit, Unit> ButtonAddNewArticle { get; }
    public string InputArticleNumber { get; set; }
    public string InputArticleName { get; set; }
    public string InputArticleDescription { get; set; }
    public string InputArticlePrice { get; set; }
    private string _header = "";
    public string Header
    {
        get => _header;
        set => this.RaiseAndSetIfChanged(ref _header, value);
    }
    
    private string _subheader = "";

    public string Subheader
    {
        get => _subheader;
        set => this.RaiseAndSetIfChanged(ref _subheader, value);
    }

    public NewArticleViewModel()
    {
        ButtonAddNewArticle = ReactiveCommand.Create(() => ButtonSafeNewArticle());
        Header = $"Neuer Artikel";
    }

    public void ButtonSafeNewArticle()
    {
        if (!string.IsNullOrWhiteSpace(InputArticleNumber))
        {
            Article newArticle = new Article(
                1,
                InputArticleNumber,
                InputArticleName ?? "",
                InputArticleDescription ?? "",
                InputArticlePrice ?? ""
            );
            var db = new Database.Database();
            db.AddArticle(newArticle);

            var articlevm = new AllArticlesViewModel();
            articlevm.CreateArticleList();
            articlevm.Navigate = Navigate; 
            Navigate?.Invoke(articlevm);
        }
        else
        {
            Subheader = "❌ Artikelnummer fehlt";
        }
    }
    

}