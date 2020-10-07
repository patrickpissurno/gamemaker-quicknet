namespace QuickNet
{
    // each char can be stored with 6 bits, a 25% reduced size compared to ASCII
    // possible characters: a-z 0-9 _ ! [] {}
    // new chars can be added in the future, as the only used values are the range of
    // [0, 41] and the 63 that represents the garbage value (a.k.a., no char at all)
    internal static class SubASCIIStringEncoder
    {
        private static readonly byte[] asciiToIdentifier = new byte[255];
        private static readonly char[] identifierToAscii = new char[64];
        static SubASCIIStringEncoder()
        {
            var a = "0123456789abcdefghijklmnopqrstuvwxyz_![]{}";
            var b = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_![]{}";
            for (var i = 0; i < asciiToIdentifier.Length; i++)
            {
                var pos = a.IndexOf((char)i);
                if (pos == -1)
                    pos = b.IndexOf((char)i);
                if (pos == -1)
                    pos = a.IndexOf('_');
                asciiToIdentifier[i] = (byte)pos;
            }

            for (var i = 0; i < a.Length; i++)
                identifierToAscii[i] = a[i];
        }

        public static byte[] GetBytes(string data)
        {
            const byte garbage = 0x3F; //00111111

            //size math is the same as Math.Ceiling(0.75f * data.length)
            var data_len = data.Length;
            var size = data_len * 3;
            size = (size % 4 == 0 ? 0 : 1) + (size / 4);

            var buffer = new byte[size];

            byte c, d;
            int t;
            for (int i = 0, b = 0; i < data_len && b < size; i++, b++)
            {
                c = asciiToIdentifier[data[i]];
                d = i + 1 >= data_len ? garbage : asciiToIdentifier[data[i + 1]];

                t = b % 3;
                if (t == 0) // [c][c][c][c][c][c][d][d]
                {
                    buffer[b] = (byte)((c << 2) + (d >> 4));
                }
                else if (t == 1) // [c][c][c][c][d][d][d][d]
                {
                    buffer[b] = (byte)((c << 4) + (d >> 2));
                }
                else // [c][c][d][d][d][d][d][d]
                {
                    buffer[b] = (byte)((c << 6) + d);
                    i++;
                }
            }

            return buffer;
        }

        public static string GetString(byte[] buffer)
        {
            //bitmasks (for 'and')
            const byte t0_right = 0x3;  //00000011
            const byte t1_left = 0xF0;  //11110000
            const byte t1_right = 0xF;  //00001111
            const byte t2_left = 0xC0;  //11000000
            const byte t2_right = 0x3F; //00111111
            //bitmask (for 'or')
            const byte garbage = 0xC0;  //11000000

            var bytes_len = buffer.Length;

            var result = "";

            byte c, d;
            bool last_byte;
            int t;
            for (var b = 0; b < bytes_len; b++)
            {
                t = b % 3;

                last_byte = b == bytes_len - 1;

                if (t == 0) // [c][c][c][c][c][c][d][d]
                {
                    c = (byte)(buffer[b] >> 2);
                    result += identifierToAscii[c];

                    if (!last_byte) //if last_byte, then d is garbage
                    {
                        d = (byte)
                        (
                            (
                                (buffer[b] & t0_right) << 4
                            )
                            +
                            (
                                (buffer[b + 1] & t1_left) >> 4 //'& t1_left' can be safely removed
                            )
                        );
                        result += identifierToAscii[d];
                    }
                }
                else if (t == 1) // [c][c][c][c][d][d][d][d]
                {
                    if (!last_byte) //if last_byte, then d is garbage
                    {
                        d = (byte)
                        (
                            (
                                (buffer[b] & t1_right) << 2
                            )
                            +
                            (
                                (buffer[b + 1] & t2_left) >> 6 //'& t2_left' can be safely removed
                            )
                        );
                        result += identifierToAscii[d];
                    }
                }
                else // [c][c][d][d][d][d][d][d]
                {
                    d = (byte)(buffer[b] & t2_right);

                    if (!last_byte || (d | garbage) < 255) // if d is garbage, then (d | garbage) = 255
                        result += identifierToAscii[d];
                }
            }

            return result;
        }
    }
}
