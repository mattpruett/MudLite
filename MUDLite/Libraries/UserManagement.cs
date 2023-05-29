using MattPruett.MUDLite.Data;
using MattPruett.MUDLite.GameObjects;
using MattPruett.MUDLite.System;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MattPruett.MUDLite.Libraries
{
    internal static class UserManagement
    {
        public static bool AuthenticateCredentials(Client client)
        {
            var userName = client.State[StateKeys.UserName]?.ToString();
            var password = client.State[StateKeys.UserPassword]?.ToString();

            using (var db = new MUDLiteDataContext())
            {
                var user = (
                    from usr in db.Users
                    where usr.Name == userName
                       && usr.Password == password
                    select usr).FirstOrDefault();

                client.User = user == null ? null : new User(user);
                client.User?.SetClient(client);
                return client.User != null;
            }
        }

        public static Data.DataModel.Models.Tbl_User CreateUser(string userName, string password)
        {
            using (var db = new MUDLiteDataContext())
            {
                var user = new Data.DataModel.Models.Tbl_User
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = userName,
                    Password = password
                };
                db.Users.Add(user);
                db.SaveChanges();

                return user;
            }
        }

        private static void GoBackToLogin(Client client)
        {
            client.Status = ClientStatus.CreatingUser;
            SendWelcomeMessage(client);
        }

        public static void HandleLogin(Client client, string message)
        {
            var words = (message ?? string.Empty).ToArray();
            switch (client.Status)
            {
                case ClientStatus.CreatingUser:
                case ClientStatus.Guest:
                    HandleUserGuestEntry(client, words);
                    break;

                case ClientStatus.EnteringUserName:
                case ClientStatus.CreatingUserName:
                    HandleUserNameEntry(client, words);
                    break;

                case ClientStatus.EnteringUserPassword:
                case ClientStatus.CreatingUserPassword:
                case ClientStatus.ConfirmingUserPassword:
                    HandleUserPasswordEntry(client, words);
                    break;

                // Assume character creation
                default:
                    CharacterCreation.HandleCharacterCreationManagement(client, message);
                    break;
            }
        }

        private static void HandleUserGuestEntry(Client client, string[] words)
        {
            var message = words[0]?.TrimLower();
            switch (message)
            {
                case "new":
                    client.Send(
                        Constants.END_LINE,
                        "User name: "
                    );
                    client.Status = ClientStatus.CreatingUserName;
                    break;

                case "login":
                    client.Send(
                        Constants.END_LINE,
                        "User name: "
                    ); 
                    client.Status = ClientStatus.EnteringUserName;
                    break;

                default:
                    client.Send(
                        Constants.END_LINE,
                        "Invalid option!"
                    );
                    SendWelcomeMessage(client);
                    break;
            }
        }

        private static void HandleUserNameEntry(Client client, string[] words)
        {
            // If it's multiple words or no words at all, go back to the beginning.
            if (words.Length != 1 || string.IsNullOrEmpty(words[0]))
            {
                GoBackToLogin(client);
                return;
            }

            var userName = words[0];

            // TODO: We should add a profanity filter here.
            client.State[StateKeys.UserName] = userName;
            client.Send(Constants.END_LINE, "Password: ");

            switch (client.Status)
            {
                case ClientStatus.EnteringUserName:
                    client.Status = ClientStatus.EnteringUserPassword;
                    break;
                case ClientStatus.CreatingUserName:
                    client.Status = ClientStatus.CreatingUserPassword;
                    break;
            }
        }

        private static void HandleUserPasswordEntry(Client client, string[] words)
        {
            // If it's multiple words or no words at all, go back to the beginning.
            if (words.Length != 1 || string.IsNullOrEmpty(words[0]))
            {
                GoBackToLogin(client);
                return;
            }

            var hashedPassword = Crypto.HashString(words[0]);

            switch (client.Status)
            {
                case ClientStatus.EnteringUserPassword:
                    // Determine if user exists with password
                    client.State[StateKeys.UserPassword] = hashedPassword;
                    client.Send(Constants.END_LINE, "Authenticating...");

                    HandleUserHasAuthenticated(client);
                    break;

                case ClientStatus.CreatingUserPassword:
                    client.State[StateKeys.UserPassword] = hashedPassword;
                    client.Send(Constants.END_LINE, "Confirm password: ");
                    client.Status = ClientStatus.ConfirmingUserPassword;
                    break;

                case ClientStatus.ConfirmingUserPassword:

                    var userName = client.State[StateKeys.UserName].ToString();

                    // Check here to ensure user doesn't already exist with that name.
                    // We won't inform the user if the name already exists in order to prevent
                    // a malicious user trying to query the system for user names.
                    var prevPassword = client.State[StateKeys.UserPassword].ToString();
                    if (!UserNameAlreadyExists(userName) && prevPassword == hashedPassword)
                    {
                        client.User = new User(CreateUser(userName, hashedPassword));

                        client.Send("User successfully created. Welcome", userName, ".", Constants.END_LINE, "Please enter a character name: ");
                        client.Status = ClientStatus.CreatingCharacterName;
                    }
                    else
                    {
                        client.Send("Invalid username or passwords do not match.", Constants.END_LINE);
                        GoBackToLogin(client);
                    }
                    break;
            }
        }

        public static void SendWelcomeMessage(Client client)
        {
            client.Send(
                Constants.END_LINE,
                "Welcome to MUDLite",
                Constants.END_LINE,
                "If you are new or would like to create a new user enter \"New\" or enter \"Login\" to log in.",
                Constants.END_LINE,
                Constants.CURSOR
            );
        }

        private static void HandleUserHasAuthenticated(Client client)
        {
            if (AuthenticateCredentials(client))
            {
                Globals.Server.ClearClientScreen(client);
                client.Send("Successfully authenticated.");
                CharacterCreation.GoBackToCharacterSelection(client);
            }
            else
            {
                client.Send("Invalid credentials!", Constants.END_LINE);
                GoBackToLogin(client);
            }
        }

        public static bool UserNameAlreadyExists(string userName, string id = null)
        {
            using (var db = new MUDLiteDataContext())
            {
                id = id ?? string.Empty;
                try
                {
                    var exists = (from usr in db.Users
                                  where usr.Name == userName
                                     && usr.Id != id
                                  select true).Any();
                    return exists;
                }
                catch
                {
                    // Om nom nom nom.
                    return false;
                }
            }
        }
    }
}
