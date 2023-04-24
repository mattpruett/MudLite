namespace MattPruett.MUDLite.Libraries
{
    internal class Constants
    {
        public const string Title = END_LINE +
@"███    ███ ██    ██ ██████  ██      ██ ████████ ███████ 
████  ████ ██    ██ ██   ██ ██      ██    ██    ██      
██ ████ ██ ██    ██ ██   ██ ██      ██    ██    █████   
██  ██  ██ ██    ██ ██   ██ ██      ██    ██    ██      
██      ██  ██████  ██████  ███████ ██    ██    ███████" + END_LINE;
        public const string END_LINE = "\r\n";
        public const string CURSOR = " > ";

        // TODO: Make this path relative
        public const string DataLocation = @"C:\Users\kaoso\Documents\GitHub\MUDLite\MUDLite\Data";
        public const string DatabaseName = "MUDLite.db";
        // TODO: and add db file to publish profile so it's placed in the bin folder.
        public const string Database = DataLocation + "\\" + DatabaseName;
    }

    internal class Terminal
    {
        public const string ClearScreen = "\u001B[1J\u001B[H";

        public static byte[] NewClientMessage = 
            {
                0xff, 0xfd, 0x01,   // Do Echo
                0xff, 0xfd, 0x21,   // Do Remote Flow Control
                0xff, 0xfb, 0x01,   // Will Echo
                0xff, 0xfb, 0x03    // Will Supress Go Ahead
            };

        internal class ByteConst
        {
            public const byte Period = 0x2E;
            public const byte CR = 0x0D;
            public const byte LF = 0x0A;
            public const byte BackSpace = 0x08;
            public const byte Space = 0x20;
            public const byte Delete = 0x7F;
            public const byte ú = 0xfA;
        }
    }
}
