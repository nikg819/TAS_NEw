using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reflection.Metadata.Ecma335;
using ReactiveUI;
using TAS_Test.Models;
using TAS_Test.Views;

namespace TAS_Test.ViewModels;

public class AllArticlesViewModel : ReactiveObject
{
    public Action<ReactiveObject>? Navigate { get; set; }
    private ObservableCollection<Article> _allArticles = new();
    public ObservableCollection<Article> AllArticles
    {
        get => _allArticles;
        set => this.RaiseAndSetIfChanged(ref _allArticles, value);
    }

    private string _subheader = "";
    public string Subheader
    {
        get => _subheader;
        set => this.RaiseAndSetIfChanged(ref _subheader, value);
    }
    
    private string _searchText;
    public string SearchText
    {
        get => _searchText;
        set => this.RaiseAndSetIfChanged(ref _searchText, value);
    }
    public ReactiveCommand<string, Unit> SearchCommand { get; }
    public ReactiveCommand<Article, Unit> EditCommand { get; }
    public ReactiveCommand<Article, Unit> DeleteCommand { get; }
    public ReactiveCommand<Unit, Unit> NewArticle { get; }

    public AllArticlesViewModel()
    {
        EditCommand = ReactiveCommand.Create<Article>(EditArticle);
        DeleteCommand = ReactiveCommand.Create<Article>(DeleteArticle);
        NewArticle = ReactiveCommand.Create(AddNewArticle);

        CreateArticleList();
    }

    private void EditArticle(Article article)
    {
        Console.WriteLine("EditArticle");
    }

    private void DeleteArticle(Article article)
    {
        string message = $"Willst du {article.ArticleName} löschen?";
        var infowindow = new InfoWindow();
        infowindow.DataContext = new InfoViewModel(infowindow, message, article.ArticleDatabaseId,this);
        infowindow.Show();
    }

    private void AddNewArticle()
    {
        var newarticlevm = new NewArticleViewModel();
        newarticlevm.Navigate = Navigate;
        Navigate?.Invoke(newarticlevm);
        
    }

    public void CreateArticleList()
    {
        var db = new Database.Database();
        var allArticles = db.GetAllArticles();
        AllArticles = new ObservableCollection<Article>(allArticles);
        Subheader = $"Anzahl Artikel: {allArticles.Count}";
    }
}