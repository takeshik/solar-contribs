// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* CacheUtils
 *   Extensional library for Solar, StatusCache utility
 * Copyright © 2010 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of CacheUtils.
 * 
 * This library is dual-licensed, under the MIT License, and the same conditions
 * which copyright holder of Solar prescribes. You can use this library and its
 * source codes under the license you selected.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Lunar;

namespace XSpect.Contributions.Solar
{
    public static class CacheDumper
    {
        private const String H = "{http://www.w3.org/1999/xhtml}";

        public static String Export(IEnumerable<Status> statuses)
        {
            return String.Join(Environment.NewLine, statuses
                .OrderByDescending(s => s.CreatedAt)
                .Select(s => String.Format("{0}:{1} [{2}]", s.UserName, s.Text, s.Uri))
            );
        }

        public static XElement Dump(IEnumerable<Status> statuses)
        {
            return new XElement("Statuses", statuses.Select(Dump));
        }

        public static XElement Dump(Status status)
        {
            return new XElement("Status",
                new XAttribute("StatusID", status.StatusID.ToString()),
                new XAttribute("CreatedAt", status.CreatedAt.ToString("s")),
                new XAttribute("UserID", ((IID) status.UserID).Value != 0 ? status.UserID.ToString() : ""),
                new XAttribute("UserName", status.UserName),
                new XAttribute("SourceName", status.SourceName),
                new XAttribute("flags", String.Join(", ", new[]
                {
                    status.IsDirectMessage ? "DirectMessage" : null,
                    status.Favorited ? "Favorited" : null,
                    status.IsOwned ? "Owned" : null,
                    status.Protected ? "Protected" : null,
                    status.IsReply ? "Reply" : null,
                    status.IsRetweet ? "Retweet" : null,
                    status.IsSearchResult ? "SearchResult" : null,
                    status.Truncated ? "Truncated" : null
                }.Where(_ => _ != null))),
                status.Text
            );
        }

        public static XElement Dump(IEnumerable<User> users)
        {
            return new XElement("Users", users.Select(Dump));
        }

        public static XElement Dump(User user)
        {
            return new XElement("User",
                new XAttribute("UserID", ((IID) user.UserID).Value != 0 ? user.UserID.ToString() : ""),
                new XAttribute("Name", user.Name ?? ""),
                new XAttribute("FullName", user.FullName ?? ""),
                new XAttribute("Location", user.Location ?? ""),
                new XAttribute("Description", user.Description ?? ""),
                new XAttribute("WebSite", user.WebSite != null ? user.WebSite.ToString() : ""),
                new XAttribute("CreatedAt", user.CreatedAt.ToString("s")),
                new XAttribute("StatusesCount", user.StatusesCount),
                new XAttribute("FollowersCount", user.FollowersCount),
                new XAttribute("FollowingCount", user.FollowingCount),
                new XAttribute("FavouritesCount", user.FavouritesCount),
                new XAttribute("flags", String.Join(", ", new[]
                {
                    user.Protected ? "Protected" : null,
                    user.Verified ? "Verified" : null
                }.Where(_ => _ != null)))
            );
        }

        public static XElement Format(IEnumerable<Status> statuses)
        {
            return new XElement(H + "ul",
                statuses.Select(s =>
                    new XElement(H + "li",
                        new XElement(H + "a",
                            new XAttribute("href", s.Uri),
                            s.CreatedAt.ToString("G")
                        ),
                        " ",
                        new XElement(H + "a",
                            new XAttribute("href", "https://twitter.com/" + s.UserName),
                            String.Format("{0} ({1})", s.UserName, s.FullUserName)
                        ),
                        s.Protected ? " *PROTECTED*" : "",
                        new XElement(H + "br"),
                        s.Text
                    )
                )
            );
        }

        public static XElement Format(Status status)
        {
            return new XElement("Status",
                new XAttribute("StatusID", status.StatusID.ToString()),
                new XAttribute("CreatedAt", status.CreatedAt.ToString("s")),
                new XAttribute("UserID", ((IID) status.UserID).Value != 0 ? status.UserID.ToString() : ""),
                new XAttribute("UserName", status.UserName),
                new XAttribute("SourceName", status.SourceName),
                new XAttribute("flags", String.Join(", ", new[]
                {
                    status.IsDirectMessage ? "DirectMessage" : null,
                    status.Favorited ? "Favorited" : null,
                    status.IsOwned ? "Owned" : null,
                    status.Protected ? "Protected" : null,
                    status.IsReply ? "Reply" : null,
                    status.IsRetweet ? "Retweet" : null,
                    status.IsSearchResult ? "SearchResult" : null,
                    status.Truncated ? "Truncated" : null
                }.Where(_ => _ != null))),
                status.Text
            );
        }
    }
}
