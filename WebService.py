import clr
clr.AddReferenceToFile("WebService.dll")
from System import *
from System.Windows.Controls import *
from Solar import *
from Solar.Models import *
from XSpect.Contributions.Solar import *

### 設定 ここから:

# listen する IP アドレス
WebService.Instance.Configuration.ListenAddress = "127.0.0.1"
# listen するポート番号
WebService.Instance.Configuration.ListenPort = 8080

### 設定 ここまで

startMenu = MenuItem()
stopMenu = MenuItem()

def Load():
    parentMenu = App.Current.MainWindow.MainMenu.Items[3]
    startMenu.Header = 'Web サーバの開始'
    startMenu.Click += StartWebService
    parentMenu.Items.Add(startMenu)
    stopMenu.Header = 'Web サーバの停止'
    stopMenu.Click += StopWebService
    parentMenu.Items.Add(stopMenu)
    stopMenu.IsEnabled = False

def StartWebService(sender, e):
    WebService.Instance.Start()
    startMenu.IsEnabled = False
    stopMenu.IsEnabled = True

def StopWebService(sender, e):
    WebService.Instance.Stop()
    startMenu.IsEnabled = True
    stopMenu.IsEnabled = False

App.Current.Dispatcher.Invoke(Action(Load))
