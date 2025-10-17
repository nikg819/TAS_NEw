using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reflection.Metadata.Ecma335;
using ReactiveUI;
using TAS_Test.Models;

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

    public AllArticlesViewModel()
    {
        Subheader = "Anzahl Artikel: 1";
        
        EditCommand = ReactiveCommand.Create<Article>(EditArticle);
        DeleteCommand = ReactiveCommand.Create<Article>(DeleteArticle);

        CreateArticleList();
    }

    private void EditArticle(Article article)
    {
        Console.WriteLine("EditArticle");
    }

    private void DeleteArticle(Article article)
    {
        Console.WriteLine("DeleteArticle");
    }

    private void CreateArticleList()
    {
        var db = new Database.Database();
        var allArticles = db.GetAllArticles();
        AllArticles = new ObservableCollection<Article>(allArticles);
    }
}