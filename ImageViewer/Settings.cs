﻿// TODO
// Add file watcher to settings file, to reload or create if deleted.
// Handle parsing errors, like if something is spelled wrong.
// Handle missing hotkeys or similar.
using System;
using System.Windows.Input;
using System.Collections.Generic;

namespace ImageViewer
{

    public class Settings
    {
        private const string SettingsFilename = "Settings.xml";
        private const string DefaultSettings =
@"<?xml version=""1.0"" encoding=""utf-8""?>
<Settings>
    <Colors Background = ""32, 32, 32"" />
    <Hotkeys>
        NextImage = ""Right""
        PreviousImage = ""Left""
        DeleteImage = ""Delete""
    </Hotkeys>
</Settings>";
        public Dictionary<Commands, Key> Hotkeys = new Dictionary<Commands, Key>();

        public System.Windows.Media.SolidColorBrush Background;
        public System.Drawing.Color BackgroundColor;

        public void Load()
        {

            if (!System.IO.File.Exists(SettingsFilename))
            {
                System.IO.File.WriteAllText(SettingsFilename, DefaultSettings);
            }

            ReadSettingsFile();

        }

        private void ReadSettingsFile()
        {
            using (var SettingsReader = System.Xml.XmlReader.Create(SettingsFilename))
            {
                while (SettingsReader.Read())
                {
                    if (SettingsReader.IsStartElement())
                    {
                        switch (SettingsReader.Name)
                        {
                            case "Colors":
                                while (SettingsReader.MoveToNextAttribute())
                                {
                                    switch (SettingsReader.Name)
                                    {
                                        case "Background":
                                            var color = RGBColorFromCSV(SettingsReader.GetAttribute(SettingsReader.Name));
                                            BackgroundColor = System.Drawing.Color.FromArgb(color.R, color.G, color.B);
                                            Background = new System.Windows.Media.SolidColorBrush(color);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                SettingsReader.MoveToElement();
                                break;
                            case "Hotkeys":
                                while (SettingsReader.MoveToNextAttribute())
                                {
                                    switch (SettingsReader.Name)
                                    {
                                        case "NextImage":
                                            Hotkeys.Add(Commands.NextImage, StringToKey(SettingsReader.Value));
                                            break;
                                        case "PreviousImage":
                                            Hotkeys.Add(Commands.PreviousImage, StringToKey(SettingsReader.Value));
                                            break;
                                        case "DeleteImage":
                                            Hotkeys.Add(Commands.DeleteImage, StringToKey(SettingsReader.Value));
                                            break;
                                        default:
                                            // TODO Add a message box and quit the program.
                                            throw new Exception($"Invalid Hotkey: {SettingsReader.Name} : {SettingsReader.Value}");
                                    }
                                }
                                SettingsReader.MoveToElement();
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        private static System.Windows.Media.Color RGBColorFromCSV(string cvs_string)
        {
            var background_raw = cvs_string.Split(',');
            return System.Windows.Media.Color.FromRgb(byte.Parse(background_raw[0]), byte.Parse(background_raw[1]), byte.Parse(background_raw[2]));
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        private Key StringToKey(string s)
        {
            // TODO Add more keys.
            switch (s)
            {
                case "Right":
                    return Key.Right;
                case "Left":
                    return Key.Left;
                case "Delete":
                    return Key.Delete;
                default:
                    throw new KeyNotFoundException("Invalid Key");
            }
        }
    }
}