﻿using System;
using System.Collections.Generic;
using System.Linq;
using PlayFabPowerTools.Services;

namespace PlayFabPowerTools.Packages
{
    public class PlayFabLoginPackage : iStatePackage
    {
        private string _username;
        private string _password;
        private static string _DeveloperClientToken;
        
        private enum States
        {
            Idle,
            WaitForUsername,
            WaitForPassword,
            Login,
            LoginAAD,
            GetStudios
        }

        private States _state;

        public void RegisterMainPackageStates(iStatePackage package)
        {
            List<MainPackageStates> states = new List<MainPackageStates>()
            {
                MainPackageStates.Login,
                MainPackageStates.LoginAAD,
                MainPackageStates.Logout
            };
            PackageManagerService.RegisterMainPackageStates(states, package);
        }

        public bool SetState(string line)
        {
            if(line.ToLower().Contains("loginaad"))
            {
                _state = States.LoginAAD;
                return false;
            }

            if (line.ToLower().Contains("auto"))
            {
                var argsList = line.Split(' ');
                var credentials = argsList.ToList().FindAll(s => !s.ToLower().Contains("login") && !s.ToLower().Contains("auto")).ToList();
                if (credentials.Count < 2)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.White;
                    _state = States.Idle;
                    return false;
                }
                _username = credentials[0];
                _password = credentials[1];
                _state = States.Login;
                return false;
            }

            if (_state == States.Idle)
            {
                _state = States.WaitForUsername;
            }else if (_state == States.WaitForUsername)
            {
                _username = line;
                _state = States.WaitForPassword;
                return true;
            }
            else if (_state == States.WaitForPassword)
            {
                _password = line;
                _state = States.Login;
            }else if (_state == States.Login)
            {
                _state = States.GetStudios;
            }
            return false;
        }

        public bool Loop()
        {
            var waitForLogin = false;
            switch (_state)
            {
                case States.WaitForUsername:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("");
                    Console.WriteLine("Username:");
                    
                    break;
                case States.WaitForPassword:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("");
                    Console.WriteLine("Password:");
                    break;
                case States.Login:
                    waitForLogin = true;
                    PlayFabService.Login(_username, _password, (success, devKey) =>
                    {
                        if (!success)
                        {
                            PackageManagerService.SetState(MainPackageStates.Idle);
                            _state = States.Idle;
                            waitForLogin = false;
                            return;
                        }

                        _DeveloperClientToken = devKey;
                        Console.ForegroundColor = ConsoleColor.White;
                        _state = States.GetStudios;
                        Loop();
                        waitForLogin = false;
                    });
                    break;
                 case States.LoginAAD:
                    waitForLogin = true;
                    PlayFabService.LoginAAD((success, devKey) =>
                    {
                        if (!success)
                        {
                            PackageManagerService.SetState(MainPackageStates.Idle);
                            _state = States.Idle;
                            waitForLogin = false;
                            return;
                        }

                        _DeveloperClientToken = devKey;
                        Console.ForegroundColor = ConsoleColor.White;
                        _state = States.GetStudios;
                        Loop();
                        waitForLogin = false;
                    });
                    break;
                case States.GetStudios:
                    waitForLogin = true;
                    PlayFabService.GetStudios(_DeveloperClientToken, (success) =>
                    {
                        PackageManagerService.SetState(MainPackageStates.Idle);
                        _state = States.Idle;
                        waitForLogin = false;
                    });
                    break;
                case States.Idle:
                default:
                    break;
            }

            //TODO: Paul Help?
            do
            {
                //Block util login call is done. 
            } while (waitForLogin);

            return false;
        }
    }
}
