# bit.ly url shortener
from System import *
from System.Net import *

def Shorten(uri = Uri):
    uri = uri.AbsoluteUri
    if uri.Length <= "http://j.mp/XXXXXX".Length:
        return uri
    Login = "USERNAME"
    ApiKey = "APIKEY"
    with WebClient() as wc:
        return wc.DownloadString("http://api.j.mp/v3/shorten?format=txt&login=" + Login + "&apiKey=" + ApiKey + "&longUrl=" + Uri.EscapeDataString(uri)) \
            .TrimEnd(Environment.NewLine.ToCharArray())
