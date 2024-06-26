﻿using Notification.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Notification.Models
{
    public static class NotificationExtesions
    {
        public static bool HasErrors(this IList<NotificationMessage> messages)
        {
            return messages.Count(m => m.Type == ENotificationType.Error) > 0;
        }

        public static void RemoveDuplicateMessages(this List<NotificationMessage> messages)
        {
            var newErrorList = (from i in messages
                                select i).Distinct(new NotificationCompar()).ToList();

            messages.Clear();
            messages.AddRange(newErrorList);
        }
    }
}