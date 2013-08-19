using System.Collections.Generic;

namespace SmfLite
{
    public struct FileContainer
    {
        public int division;
        public List<Track> tracks;

        public FileContainer (int division, List<Track> tracks)
        {
            this.division = division;
            this.tracks = tracks;
        }

        public override string ToString ()
        {
            var temp = division.ToString () + ",";
            foreach (var track in tracks) {
                temp += track;
            }
            return temp;
        }
    }
}