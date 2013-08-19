using System.Collections.Generic;

namespace SmfLite
{
    public static class FileLoader
    {
        public static FileContainer Load (byte[] data)
        {
            var tracks = new List<Track> ();
            var reader = new StreamReader (data);

            // Chunk type.
            if (new string (reader.ReadChars (4)) != "MThd") {
                throw new System.FormatException ("Can't find header chunk.");
            }

            // Chunk length.
            if (reader.ReadBEInt32 () != 6) {
                throw new System.FormatException ("Length of header chunk must be 6.");
            }

            // Format (unused).
            reader.Advance (2);

            // Number of tracks.
            var trackCount = reader.ReadBEInt16 ();

            // Delta-time divisions.
            var division = reader.ReadBEInt16 ();
            if ((division & 0x8000) != 0) {
                throw new System.FormatException ("SMPTE time code is not supported.");
            }

            // Read the tracks.
            for (var trackIndex = 0; trackIndex < trackCount; trackIndex++) {
                tracks.Add (ReadTrack (reader));
            }

            return new FileContainer (division, tracks);
        }

        static Track ReadTrack (StreamReader reader)
        {
            var track = new Track ();

            // Chunk type.
            if (new string (reader.ReadChars (4)) != "MTrk") {
                throw new System.FormatException ("Can't find track chunk.");
            }
            
            // Chunk length.
            var chunkEnd = reader.Offset + reader.ReadBEInt32 ();

            // Read delta-time and event pairs.
            while (reader.Offset < chunkEnd) {
                // Delta time.
                var delta = reader.ReadMultiByteValue ();

                // Event type.
                byte ev = reader.ReadByte ();

                if (ev == 0xff) {
                    // 0xff: Meta event (unused).
                    reader.Advance (1);
                    reader.Advance (reader.ReadMultiByteValue ());
                } else if (ev == 0xf0) {
                    // 0xf0: SysEx (unused).
                    while (reader.ReadByte() != 0xf7) {
                    }
                } else {
                    // MIDI event
                    byte data1 = reader.ReadByte ();
                    byte data2 = ((ev & 0xe0) == 0xc0) ? (byte)0 : reader.ReadByte ();
                    track.AddDeltaAndMessage (delta, new Message (ev, data1, data2));
                }
            }
               
            return track;
        }
    }
}