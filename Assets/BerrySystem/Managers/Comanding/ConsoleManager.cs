/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;
using System;
using System.Text;

namespace ConstruiSystem
{
    public static class ConsoleManager
    {
        public static CSConsole consoleReference;
        public static string[] log;
        public static int logMaxCount;

        public static commandSet consoleBaseCommands;

        public static void CreateConsole()
        {
            registerBaseCommands();
            if (consoleReference != null)
            {
                if (consoleReference.isActiveAndEnabled)
                {
                    return;
                }
                else
                {
                    consoleReference.gameObject.SetActive(true);
                    return;
                }
            }
            consoleReference = new GameObject("Console").AddComponent<CSConsole>();
            consoleReference.CreateConsole();
        }

        public static void ConsoleUpdateDisplay()
        {

        }
        public static void LogString(string arg)
        {
            string[] log = new string[1];
            log[0] = arg;
            Log(log);
        }

        public static void Log(string[] args)
        {
            if (args.Length != 0)
            {
                Debug.Log("CS LOG: " + args[0]);
            }
            else
            {
                Debug.Log("");
            }
        }

        // This function takes raw user console input and issues commands accordingly.
        public static void Command(string command)
        {
            commandData[] commands = commandParse(command);
            for (int i = 0; i < commands.Length; i++)
            {
                // Query the base commands first.
                CommandQuery(consoleBaseCommands, commands[i]);
            }
            /*  DEBUG INFO
            for (int i = 0; i < commands.Length; i++)
            {
                Debug.Log(commands[i].commandID)lol
                for (int j = 0; j < commands[i].args.Length; j++)
                {
                    Debug.Log(commands[i].args[j]);
                }
            }
             */
        }

        public static void CommandQuery(commandSet targetCommandSet, commandData targetCommand)
        {
            if (targetCommandSet != null)
            {
                for (int i = 0; i < targetCommandSet.commands.Length; i++)
                {
                    if (targetCommand.commandID == targetCommandSet.commands[i].commandID)
                    {
                        targetCommandSet.commands[i].onCommandCall.Invoke(targetCommand.args);
                    }
                }
            }
        }

        // This function iterates over raw user input and breaks it down to valid commandData.
        public static commandData[] commandParse(string command)
        {
            string[] comStrings = new string[0];

            // Separate all words by " " spaces into divided strings.
            bool inString = false;
            bool stringBounty = false;
            bool sbFirst = false;
            char[] newString = new char[0];
            for (int i = 0; i < command.Length; i++)
            {
                if (command[i].ToString() == "\"" && !stringBounty)
                {
                    stringBounty = true;
                    sbFirst = true;
                }
                if (command[i].ToString() == " " && !stringBounty)
                {
                    if (inString)
                    {
                        if (comStrings.Length != 0)
                        {
                            comStrings[comStrings.Length - 1] = new string(newString);
                        }
                    }
                    inString = false;
                }
                else
                {
                    if (!inString)
                    {
                        newString = new char[0];
                        Array.Resize(ref comStrings, comStrings.Length + 1);
                    }
                    inString = true;
                    Array.Resize(ref newString, newString.Length + 1);
                    newString[newString.Length - 1] = command[i];
                }
                if (stringBounty && command[i].ToString() == "\"" && !sbFirst)
                {
                    stringBounty = false;
                }
                if (command.Length - 1 == i)
                {
                    if (inString)
                    {
                        if (comStrings.Length != 0)
                        {
                            comStrings[comStrings.Length - 1] = new string(newString);
                        }
                    }
                }
                sbFirst = false;
            }

            // Identify witch strings are arg/variables

            bool isVar = false;
            commandData[] parsedCommands = new commandData[0];

            for (int a = 0; a < comStrings.Length; a++)
            {
                // Identify strings.
                if (comStrings[a][0].ToString() == "\"" && comStrings[a][comStrings[a].Length - 1].ToString() == "\"")
                {
                    isVar = true;
                }

                // Identify booleans
                if (comStrings[a] == "true" || comStrings[a] == "TRUE" || comStrings[a] == "false" || comStrings[a] == "FALSE")
                {
                    isVar = true;
                }

                // Identify integers


                //Identify command specific property ( --Command-Property )
                if (comStrings[a].Length >= 3)
                {

                    if (comStrings[a][0].ToString() == "-" && comStrings[a][1].ToString() == "-")
                    {
                        isVar = true;
                    }
                }

                if (!isVar)
                {
                    Array.Resize(ref parsedCommands, parsedCommands.Length + 1);
                    parsedCommands[parsedCommands.Length - 1] = new commandData();
                    parsedCommands[parsedCommands.Length - 1].commandID = comStrings[a];
                    parsedCommands[parsedCommands.Length - 1].args = new string[0];
                }
                else
                {
                    if (parsedCommands.Length != 0)
                    {
                        Array.Resize(ref parsedCommands[parsedCommands.Length - 1].args, parsedCommands[parsedCommands.Length - 1].args.Length + 1);
                        parsedCommands[parsedCommands.Length - 1].args[parsedCommands[parsedCommands.Length - 1].args.Length - 1] = comStrings[a];
                    }
                }
                isVar = false;
            }
            return parsedCommands;
        }

        public static bool BoolParse(string arg)
        {
            // Identify booleans
            if (arg == "true" || arg == "TRUE")
            {
                return true;
            }
            if (arg == "false" || arg == "FALSE")
            {
                return false;
            }
            LogString("WARNING! BOOL CAN NOT BE IDENTIFIED!");
            return false;
        }

        public static string StringParse(string arg)
        {
            if (arg[0].ToString() == "\"" && arg[arg.Length - 1].ToString() == "\"")
            {
                char[] cleanString = new char[arg.Length - 2];
                for (int i = 1; i < arg.Length - 2; i++)
                {
                    cleanString[i - 2] = arg[i];
                }
                return cleanString.ToString();
            }
            LogString("WARNING! STRING CAN NOT BE IDENTIFIED!");
            return arg;
        }

        /*
            ----------------------------------------------------------------
               Console base system commands
            ----------------------------------------------------------------
         */

        // This command can navigate the users filesystem given the right OS admin powers.
        public static void CD(string[] args)
        {
            Debug.Log("CD REACHED ! WELCOME TO THE OPERATING SYSTEM INTERFACE !");
        }

        /*  Console base command TODO.

            console base commands
            - cd "file system filepath" --ls
            - log
            - ls type/command

            - devpowers bool  // Enable all dev comands

            - noclip 
            - god
            - notarget
            - layer bool "int layerid"
            - swithcLayer "int layerid"
            - clearall "typename"
            - killall
            - healall
            - healthmod "target entity" "int damage"
            - scale
            - pos
            - rotation 
        */

        public static void registerBaseCommands()
        {
            if (consoleBaseCommands == null)
            {
                consoleBaseCommands = new commandSet();
                consoleBaseCommands.commands = new command[2];

                consoleBaseCommands.commands[0] = new command();
                consoleBaseCommands.commands[0].commandID = "log";
                consoleBaseCommands.commands[0].onCommandCall = Log;

                consoleBaseCommands.commands[1] = new command();
                consoleBaseCommands.commands[1].commandID = "cd";
                consoleBaseCommands.commands[1].onCommandCall = CD;

            }
        }
    }

    public class commandSet
    {
        public command[] commands;
    }

    public struct commandData
    {
        public string commandID;
        public string[] args;
    }

    public struct command
    {
        public string commandID;
        public delegate void CommandCall(string[] args);
        public CommandCall onCommandCall;
    }
}