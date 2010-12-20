from System import *
from System.Windows.Controls import *
from System.Windows.Controls.Primitives import *
from Solar import *
from Solar.Models import *

p = None

def Load():
	global p
	
	f = App.Current.MainWindow
	p = StatusBarItem()
	p.Content = "Status: 0 User: 0"
	Client.Instance.Refreshed += Update
	Client.Instance.RequestedNewPage += Update
	DockPanel.SetDock(p, Dock.Right)
	f.StatusBar.Items.Insert(f.StatusBar.Items.Count - 1, p)

def Update(sender, e):
	p.Content = "Status: " + str(Client.Instance.StatusCache.StatusCount) + " User: " + str(Client.Instance.StatusCache.UserCount)

App.Current.Dispatcher.Invoke(Action(Load))
