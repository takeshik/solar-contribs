from System import *
from System.Windows.Controls import *
from System.Windows.Controls.Primitives import *
from Solar import *
from Solar.Models import *

p = None
startup = DateTime.Now

def Load():
    global p
    
    f = App.Current.MainWindow
    p = StatusBarItem()
    p.Content = "(INIT)"
    Client.Instance.Refreshed += Update
    Client.Instance.RequestedNewPage += Update
    DockPanel.SetDock(p, Dock.Right)
    f.StatusBar.Items.Insert(f.StatusBar.Items.Count - 1, p)

def Update(sender, e):
    from System.Diagnostics import Process
    p.Content = ("Up: " + (DateTime.Now - startup).ToString(("d\\.hh\\:mm\\:ss\\.ff")))

App.Current.Dispatcher.Invoke(Action(Load))