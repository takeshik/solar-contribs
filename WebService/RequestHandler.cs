// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* WebService
 *   Extensional library for Solar, provides HTTP server feature
 * Copyright © 2010 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of WebService.
 * 
 * This library is dual-licensed, under the MIT License, and the same conditions
 * which copyright holder of Solar prescribes. You can use this library and its
 * source codes under the license you selected.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using HttpServer;
using HttpServer.HttpModules;
using HttpServer.Sessions;
using Lunar;
using Solar;
using Solar.Models;

namespace XSpect.Contributions.Solar
{
    public class RequestHandler
        : HttpModule
    {
        private const String H = "{http://www.w3.org/1999/xhtml}";

        public override Boolean Process(IHttpRequest request, IHttpResponse response, IHttpSession session)
        {
            if (request.UriPath == "/")
            {
                ShowMainPage(request, response, session);
                return true;
            }
            else if (request.UriPath.StartsWith("/category/"))
            {
                var match = Regex.Match(request.UriPath, @"/category/(\d+)/(\d+)");
                ShowCategory(
                    Client.Instance.Groups[Int32.Parse(match.Groups[1].Value)][Int32.Parse(match.Groups[2].Value)],
                    request,
                    response,
                    session
                );
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void ShowMainPage(IHttpRequest request, IHttpResponse response, IHttpSession session)
        {
            Write(response, Generate("Solar Web Service",
                new XElement(H + "h1", "Categories"),
                new XElement(H + "ul",
                    Client.Instance.Groups
                        .SelectMany((g, gi) => g.Select((c, ci) =>
                            new XElement(H + "li",
                                new XElement(H + "a",
                                    new XAttribute("href", String.Format("/category/{0}/{1}/", gi, ci)),
                                    String.Format("{0}{1}", c.Name, c.Unreads > 0 ? " (" + c.Unreads + ")" : "")
                                )
                            )
                        ))
                )
            ));
        }

        private static void ShowCategory(Category category, IHttpRequest request, IHttpResponse response, IHttpSession session)
        {
            Write(response, Generate(String.Format("{0} - Solar Web Service", category.Name),
                new XElement(H + "h1", String.Format("Category: {0} ({1} / {2})", category.Name, category.Unreads, category.Statuses.Count)),
                CacheDumper.Format(category.Statuses.OfType<Status>())
            ));
        }

        private static XDocument Generate(String title, params XElement[] bodyElements)
        {
            return new XDocument(
                new XDocumentType("html", "-//W3C//DTD XHTML 1.1//EN", "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd", null),
                new XElement(H + "html",
                    new XAttribute("xmlns", "http://www.w3.org/1999/xhtml"),
                    new XElement(H + "head",
                        new XElement(H + "title", title),
                        new XElement(H + "link",
                            new XAttribute("href", "/styles/index.css"),
                            new XAttribute("rel", "stylesheet"),
                            new XAttribute("type", "text/css")
                        )
                    ),
                    new XElement(H + "body",
                        bodyElements,
                        new XElement(H + "hr"),
                        new XElement(H + "p",
                            new XAttribute("class", "footer"),
                            new XElement(H + "a",
                                new XAttribute("href", "http://9.dotpp.net/software/solar/"),
                                "Solar"
                            ),
                            String.Format(" ({0}, created by ", App.AssemblyVersion),
                            new XElement(H + "a",
                                new XAttribute("href", "http://twitter.com/mfakane"),
                                "@mfakane"
                            ),
                            ") ",
                            new XElement(H + "a",
                                new XAttribute("href", "http://github.com/takeshik/solar-scripts/"),
                                "Web Service"
                            ),
                            String.Format(" ({0}, created by ", typeof(WebService).Assembly.GetName().Version),
                            new XElement(H + "a",
                                new XAttribute("href", "http://twitter.com/takeshik"),
                                "@takeshik"
                            ),
                            ")"
                        )
                    )
                )
            );
        }

        private static void Write(IHttpResponse response, XDocument xdoc)
        {
            using (var stream = new MemoryStream())
            using (var writer = XmlWriter.Create(stream, new XmlWriterSettings()
            {
                Encoding = Encoding.UTF8,
                Indent = true,
                IndentChars = "\t",
                NewLineChars = "\n",
            }))
            {
                xdoc.WriteTo(writer);
                writer.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                Write(response, stream.ToArray(), "application/xhtml+xml");
            }

        }

        private static void Write(IHttpResponse response, Byte[] data, String contentType)
        {
            response.ContentType = contentType;
            response.Body.Write(data, 0, data.Length);
            response.Body.Flush();
        }
    }
}