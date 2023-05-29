using MattPruett.MUDLite.Data;
using MattPruett.MUDLite.System;
using System.Runtime.CompilerServices;

namespace MattPruett.MUDLite.Libraries
{
    internal static class CharacterCreation
    {
        public static void HandleCharacterCreationManagement(Client client, string message)
        {
            switch (client.Status)
            {
                case ClientStatus.ConfirmingCharacter:
                case ClientStatus.CreatingCharacterName:
                    CreateNewCharacter(client, message);
                    break;

                case ClientStatus.SelectingCharacter:
                    SelectCharacter(client, message);
                    break;
            }
        }

        public static void ListUsersCharacters(Client client)
        {
            if (client.User == null)
            {
                return;
            }

            using (var db = new MUDLiteDataContext())
            {
                var characters = client.User.Characters;
                foreach (var character in characters)
                {
                    client.Send(Constants.END_LINE, "\t", character.Name);
                }
                if (characters.Count > 0)
                {
                    client.Send(Constants.END_LINE);
                }
            }
        }

        public static void SelectCharacter(Client client, string characterName)
        {
            if (client.User == null)
            {
                return;
            }
            else if (string.IsNullOrEmpty(characterName))
            {
                GoBackToCharacterSelection(client);
            }

            if (characterName == "new")
            {
                CreateNewCharacter(client);
            }
            else
            {
                // else find character by name
                var character = client.User.CharacterByName(characterName);
                if (character != null)
                {
                    client.User.Character = character;
                    ProceedWithLogin(client);
                }
                else
                {
                    client.Send("Unable to find a character named \"", characterName, "\".", Constants.END_LINE);
                    GoBackToCharacterSelection(client);
                }
            }
        }

        public static void ShowCharacterSelectionText(Client client)
        {
            client.Send(Constants.END_LINE, "Select a character or enter \"New\" to create a new character: ");
            ListUsersCharacters(client);
            client.Status = ClientStatus.SelectingCharacter;
        }

        public static void GoBackToCharacterSelection(Client client)
        {
            var newPlayer = !client.User.HasCharacters;

            if (newPlayer)
            {
                CreateNewCharacter(client);
            }
            else
            {
                ShowCharacterSelectionText(client);
            }
        }

        public static void CreateNewCharacter(Client client)
        {
            client.Status = ClientStatus.CreatingCharacterName;
            client.Send(Constants.END_LINE, "Please enter a character name: ");
        }

        public static void CreateNewCharacter(Client client, string characterName)
        {
            var words = string.IsNullOrEmpty(characterName)
                ? new string[0]
                : characterName.ToArray();
            
            if (words.Length != 1 || words[0].ToLower() == "new")
            {
                CreateNewCharacter(client);
                return;
            }

            var name = words[0];
            switch (client.Status)
            {
                case ClientStatus.ConfirmingCharacter:
                    if (name == client.State[StateKeys.UserName].ToString())
                    {
                        client.User.CreateCharacter(name);
                        ProceedWithLogin(client);
                    }
                    else
                    {
                        client.Send(Constants.END_LINE, "The character names do not match.");
                        GoBackToCharacterSelection(client);
                    }
                    break;

                case ClientStatus.CreatingCharacterName:
                    if (DataHelpers.CharacterNameExists(name))
                    {
                        client.Send(Constants.END_LINE, "Character named \"", name, "\" already exists", Constants.END_LINE);
                        CreateNewCharacter(client);
                        return;
                    }
                    // TODO: Filter profanity and key words.
                    client.State[StateKeys.UserName] = name;
                    client.Send(Constants.END_LINE, "Confirm character name: ");
                    client.Status = ClientStatus.ConfirmingCharacter;
                    break;
            }
        }

        private static void ProceedWithLogin(Client client)
        {
            var character = client.User.Character;
            if (character != null)
            {
                client.Send(Constants.END_LINE, "You are now playing as \"", character.Name, "\". Welcome!", Constants.END_LINE);
                client.Status = ClientStatus.LoggedIn;
                client.User.ShowRoom();
            }
        }
    }
}