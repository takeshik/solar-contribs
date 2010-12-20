import clr
clr.AddReference("System.Xml")
clr.AddReference("System.Xml.Linq")
clr.AddReferenceToFile("CacheUtils.dll")
from System import *
from System.IO import *
from System.Windows.Controls import *
from System.Xml.Linq import *
from Solar import *
from Solar.Models import *
from XSpect.Contributions.Solar import *

"""
StatusCache ユーティリティ
"""
class CacheUtils:
    """
    StatusCache 内のステータスを所謂 STOT 形式でエクスポートします。Tween における "ファイル保存" 機能をシミュレートします。
    ファイルは Solar.exe と同じ位置、または Logs/ ディレクトリに作成されます。
    Returns: 作成されたファイルの、Solar.exe からの相対パス。
    """
    def Export():
        return CacheUtils._outputLog("CacheExport-", ".txt", CacheDumper.Export(Client.Instance.StatusCache.GetStatuses()))
    Export = staticmethod(Export)
    
    """
    StatusCache 内のオブジェクトを XML 形式でダンプします。
    ファイルは Solar.exe と同じ位置、または Logs/ ディレクトリに作成されます。
    Returns: 作成されたファイルの、Solar.exe からの相対パス。
    """
    def Dump():
        return CacheUtils._outputLog("CacheDump-", ".xml",
            XDocument(
                XElement("StatusCache",
                    CacheDumper.Dump(Client.Instance.StatusCache.GetStatuses()),
                    CacheDumper.Dump(Client.Instance.StatusCache.GetUsers()),
                )
            ).ToString()
        )
    Dump = staticmethod(Dump)
    
    """
    StatusCache 内のステータスを HTML 形式でダンプします。
    ファイルは Solar.exe と同じ位置、または Logs/ ディレクトリに作成されます。
    Returns: 作成されたファイルの、Solar.exe からの相対パス。
    """
    def Format():
        H = "{http://www.w3.org/1999/xhtml}"
        return CacheUtils._outputLog("CacheFormatted-", ".html",
            XDocument(
                XDocumentType("html", "-//W3C//DTD XHTML 1.1//EN", "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd", None),
                XElement(H + "html",
                    XAttribute("xmlns", "http://www.w3.org/1999/xhtml"),
                    XElement(H + "head",
                        XElement(H + "title", "Formatted Cache")
                    ),
                    XElement(H + "body",
                        CacheDumper.Format(Client.Instance.StatusCache.GetStatuses())
                    )
                )
            ).ToString()
        )
    Format = staticmethod(Format)
    
    def _outputLog(filePrefix, fileSuffix, text):
        path = ("Logs\\" if Directory.Exists("Logs") else "") + filePrefix + DateTime.Now.ToString("yyyyMMdd-HHmmss") + fileSuffix
        File.WriteAllText(path, text)
        return path
    _outputLog = staticmethod(_outputLog)