using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using ReactiveUI;
using TAS_Test.ViewModels;

namespace TAS_Test.Models;

public class Customer : ReactiveObject
{
    public int k_id {get; set;}
    public string name {get; set;}
    public string fahrzeug {get; set;}
    public string mail {get; set;}
    public string phone {get; set;}
    public string? notes {get; set;}
    public Customer() { }
    
}

