#
# CacheUtils: Utility for StatusCache
# (Originally) Written by Takeshi KIRIYA (@takeshik).
#
# Usage:
#    >>> CacheUtils.exportStatues()
#    >>> CacheUtils.dumpCache()
#    and so on.
#

import clr
clr.AddReference("System.Core")
clr.AddReference("System.Xml")
clr.AddReference("System.Xml.Linq")

from System import *
from System.Collections.Generic import *
from System.IO import *
from System.Linq import *
from System.Xml.Linq import *
from Solar import *
from Lunar import *

"""
StatusCache ユーティリティ
"""
class CacheUtils:
    """
    ステータスを所謂 STOT 形式でエクスポートします。Tween における "ファイル保存" 機能をシミュレートします。
    ファイルは Solar.exe と同じ位置、または Logs/ ディレクトリに作成されます。
    Returns: 作成されたファイルの、Solar.exe からの相対パス。
    """
    def exportStatuses():
        return _outputLog("statuslog-", ".txt",
            Enumerable.ToArray(Enumerable.Select(
                Enumerable.OrderByDescending(
                    Client.Instance.StatusCache.GetStatuses(), lambda s: s.CreatedAt
                ), lambda s: str.Format("{0}:{1} [{2}]", s.UserName, s.Text, s.Uri)
            ))
        )
    exportStatuses = staticmethod(exportStatuses)


    """
    StatusCache 内のオブジェクトを XML 形式でダンプします。
    ファイルは Solar.exe と同じ位置、または Logs/ ディレクトリに作成されます。
    Returns: 作成されたファイルの、Solar.exe からの相対パス。
    """
    def dumpCache():
        return _outputLog("cachedump-", ".xml", [
            XDocument(
                XElement("StatusCache",
                    XElement("Statuses", Enumerable.Select(Client.Instance.StatusCache.GetStatuses(), _statusSelector)),
                    XElement("Users", Enumerable.Select(Client.Instance.StatusCache.GetUsers(), _userSelector)),
                )
            ).ToString()
        ])
    dumpCache = staticmethod(dumpCache)

    def _outputLog(filePrefix, fileSuffix, lines):
        path = ("Logs\\" if Directory.Exists("Logs") else "") + filePrefix + DateTime.Now.ToString("yyyyMMdd-HHmmss") + fileSuffix
        File.WriteAllLines(path, lines)
        return path

    def _statusSelector(s):
        return XElement("Status",
            XAttribute("StatusID", s.StatusID.ToString()),
            XAttribute("CreatedAt", s.CreatedAt.ToString("s")),
            XAttribute("UserID", s.UserID.ToString() if s.UserID.Value != 0 else ""),
            XAttribute("UserName", s.UserName),
            XAttribute("SourceName", s.SourceName),
            XAttribute("flags", str.Join("",
                "DirectMessage, " if s.IsDirectMessage else "",
                "Favorited, " if s.Favorited else "",
                "Owned, " if s.IsOwned else "",
                "Protected, " if s.Protected else "",
                "Reply, " if s.IsReply else "",
                "Retweet, " if s.IsRetweet else "",
                "SearchResult, " if s.IsSearchResult else "",
                "Truncated, " if s.Truncated else "",
            )[:-2]),
            s.Text
        )

    def _userSelector(u):
        return XElement("User",
            XAttribute("UserID", u.UserID.ToString() if u.UserID.Value != 0 else ""),
            XAttribute("Name", u.Name if u.Name != None else ""),
            XAttribute("FullName", u.FullName if u.FullName != None else ""),
            XAttribute("Location", u.Location if u.Location != None else ""),
            XAttribute("Description", u.Description if u.Description != None else ""),
            XAttribute("WebSite", u.WebSite if u.WebSite != None else ""),
            XAttribute("CreatedAt", u.CreatedAt.ToString("s")),
            XAttribute("StatusesCount", u.StatusesCount),
            XAttribute("FollowersCount", u.FollowersCount),
            XAttribute("FollowingCount", u.FollowingCount),
            XAttribute("FavouritesCount", u.FavouritesCount),
            XAttribute("flags", str.Join("",
                "Protected, " if u.Protected else "",
                "Verified, " if u.Verified else "",
            )[:-2])
        )
