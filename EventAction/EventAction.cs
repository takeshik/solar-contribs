// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* EventAction
 *   Enables performing custom codes with real-time events of StatusCache
 * Copyright © 2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
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
using System.Concurrency;
using System.Linq;
using Lunar;
using Solar.Models;
using Ignition;

namespace XSpect.Contributions.Solar
{
    public static class EventAction
    {
        public static IDisposable Register(dynamic condition, dynamic action)
        {
            return Observable.FromEvent<EventArgs<Category, IList<IEntry>>>(Client.Instance, "Refreshed")
                .SelectMany(e => e.EventArgs.Value2 != null
                    ? e.EventArgs.Value2.ToObservable()
                    : Observable.Empty<IEntry>()
                )
                .Where(_ => condition(_))
                .SubscribeOn(Scheduler.TaskPool)
                .Subscribe(_ => action(_));
        }
    }
}
