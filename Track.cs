using System.Collections.Generic;

namespace SmfLite
{
    // MIDI track class.
    public class Track
    {
        public struct DeltaMessagePair
        {
            public int delta;
            public Message message;

            public DeltaMessagePair (int delta, Message message)
            {
                this.delta = delta;
                this.message = message;
            }

            public override string ToString ()
            {
                return "(" + delta.ToString ("X") + ":" + message + ")";
            }
        }

        List<DeltaMessagePair> sequence;

        public Track ()
        {
            sequence = new List<DeltaMessagePair> ();
        }

        public void AddDeltaAndMessage (int delta, Message message)
        {
            sequence.Add (new DeltaMessagePair (delta, message));
        }

        public List<DeltaMessagePair>.Enumerator GetEnumerator ()
        {
            return sequence.GetEnumerator ();
        }

        public DeltaMessagePair GetAtIndex (int index)
        {
            return sequence [index];
        }

        public override string ToString ()
        {
            var s = "";
            foreach (var pair in sequence)
                s += pair;
            return s;
        }
    }
}