using System.Collections.Generic;
using DeltaMessagePairList = System.Collections.Generic.List<SmfLite.Track.DeltaMessagePair>;

namespace SmfLite
{
    public class Sequencer
    {
        DeltaMessagePairList.Enumerator enumerator;
        bool playing;
        float pulsePerSecond;
        float pulseToNext;
        float pulseCounter;

        public bool Playing {
            get { return playing; }
        }

        public Sequencer (Track track, int ppqn, float bpm)
        {
            pulsePerSecond = bpm / 60.0f * ppqn;
            enumerator = track.GetEnumerator ();
        }

        public List<Message> Start ()
        {
            if (enumerator.MoveNext ()) {
                pulseToNext = enumerator.Current.delta;
                playing = true;
                return Advance (0);
            } else {
                playing = false;
                return null;
            }
        }

        public List<Message> Advance (float deltaTime)
        {
            if (!playing) {
                return null;
            }

            pulseCounter += pulsePerSecond * deltaTime;

            if (pulseCounter < pulseToNext) {
                return null;
            }

            var messages = new List<Message> ();

            while (pulseCounter >= pulseToNext) {
                var pair = enumerator.Current;
                messages.Add (pair.message);
                if (!enumerator.MoveNext ()) {
                    playing = false;
                    break;
                }

                pulseCounter -= pulseToNext;
                pulseToNext = enumerator.Current.delta;
            }

            return messages;
        }
    }
}