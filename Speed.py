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
    items = [i for i in Client.Instance.StatusCache.GetStatuses()
        if i != None and i.CreatedAt >= DateTime.Now - TimeSpan.FromHours(1)
    ]
    prevItems = [i for i in Client.Instance.StatusCache.GetStatuses()
        if i != None and i.CreatedAt < DateTime.Now - TimeSpan.FromHours(1) and i.CreatedAt >= DateTime.Now - TimeSpan.FromHours(2)
    ]
    accounts = [
        a.Name + ": " + str(len([s for s in items if s.UserName == a.Name])) + "p/h (" + \
        str(len([s for s in items if s.UserName == a.Name]) - len([s for s in prevItems if s.UserName == a.Name])) + ")"
        for a in Client.Instance.Accounts
    ]
    p.ToolTip = "\r\n".join(accounts)
    
    text = [s for s in accounts if s != "0p/h"]
    p.Content = ("est. " if (DateTime.Now - startup).TotalHours < 1 else "") + \
        str(len(items)) + "t/h (" + str(len(items) - len(prevItems)) + ") " + \
        (text[0].split(": ", 2)[-1] if any(text) else accounts[0].split(": ", 2)[-1]) + " " + \
        str(len([s for s in items if s.Favorited])) + "f/h (" + str(len([s for s in items if s.Favorited]) - len([s for s in prevItems if s.Favorited])) + ")"

App.Current.Dispatcher.Invoke(Action(Load))
