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
using System.Dynamic;
using System.Linq;
using System.Net;
using H = HttpServer;
using HttpServer.HttpModules;

namespace XSpect.Contributions.Solar
{
    public class WebService
    {
        private static readonly WebService _instance = new WebService();

        private readonly H.HttpServer _server;

        public static WebService Instance
        {
            get
            {
                return _instance;
            }
        }

        public dynamic Configuration
        {
            get;
            set;
        }

        private WebService()
        {
            this._server = new H.HttpServer();
            var res = new ResourceFileModule();
            res.AddResources("/", typeof(WebService).Assembly, "XSpect.Contributions.Solar.Resources.Documents");
            this._server.Add(res);
            this._server.Add(new RequestHandler());
            this.Configuration = new ExpandoObject();
        }

        public void Start()
        {
            this._server.Start(IPAddress.Parse((String) this.Configuration.ListenAddress), (Int32) this.Configuration.ListenPort);
        }

        public void Stop()
        {
            this._server.Stop();
        }
    }
}
