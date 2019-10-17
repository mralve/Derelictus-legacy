/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;
using System;

namespace ConstruiSystem
{
    // In System History we keep track of events and allow the user to undo and re do actions.
    public static class SystemHistory
    {
        public static ActionHistory[] systemHistoryList;
        public struct ActionHistory
        {
            public static string historyName;
            public static GameObject[] historyObjects;
        }

        // Register a action to the history system so it can be prossest.
        public static void HistoryRegister(string inHistoryName, GameObject[] registerObjects)
        {
            if (systemHistoryList == null)
            {
                systemHistoryList = new ActionHistory[0];
            }

            //Array.Resize(ref systemHistoryList, systemHistoryList.Length + 1);
        }
        /*
        public static T HistoryRegisterAction<T>(T param)
        {
        }
         */

        // 
        public static void HistoryUndo() { }

        // Re do a user action.
        public static void HistoryReDo() { }

    }
}