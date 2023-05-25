using CommunityToolkit.Mvvm.ComponentModel;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viewmodels;

public abstract partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    public User currentUser = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool isBusy;

    public bool IsNotBusy => !IsBusy;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotCollapsed))]
    public bool collapsed = true;

    public bool IsNotCollapsed => !Collapsed;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotConnected))]
    public bool connected = false;

    public bool IsNotConnected => !Connected;
}
