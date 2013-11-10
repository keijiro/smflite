using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SmfLite;

public class Tester : MonoBehaviour
{
	// Source MIDI file asset.
    public TextAsset sourceFile;

    // Test settings.
    public float bpm;

	// MIDI objects.
    MidiFileContainer song;
    MidiTrackSequencer sequencer;

    // Start function (MonoBehaviour).
	IEnumerator Start ()
	{
        // Load the MIDI song.
		song = MidiFileLoader.Load (sourceFile.bytes);

        // Wait for one second to avoid stuttering.
		yield return new WaitForSeconds (1.0f);

        // Start sequencing.
        ResetAndPlay (0);
	}

    // Reset and start sequecing.
    void ResetAndPlay (float startTime)
    {
        // Play the audio clip.
        audio.Play ();
        audio.time = startTime;

        // Start the sequencer and dispatch events at the beginning of the track.
        sequencer = new MidiTrackSequencer (song.tracks [0], song.division, bpm);
        DispatchEvents (sequencer.Start (startTime));
    }

    // Update function (MonoBehaviour).
    void Update ()
    {
        if (sequencer != null && sequencer.Playing) {
            // Update the sequencer and dispatch incoming events.
            DispatchEvents (sequencer.Advance (Time.deltaTime));
        }
    }

    // Dispatch incoming MIDI events. 
    void DispatchEvents (List<MidiEvent> events)
    {
        if (events != null) {
            foreach (var e in events) {
                if ((e.status & 0xf0) == 0x90) {
                    if (e.data1 == 0x24) {
                        GameObject.Find ("Kick").SendMessage ("OnNoteOn");
                    } else if (e.data1 == 0x2a) {
                        GameObject.Find ("Hat").SendMessage ("OnNoteOn");
                    } else if (e.data1 == 0x2e) {
                        GameObject.Find ("OHat").SendMessage ("OnNoteOn");
                    } else if (e.data1 == 0x26 || e.data1 == 0x27 || e.data1 == 0x28) {
                        GameObject.Find ("Snare").SendMessage ("OnNoteOn");
                    }
                }
            }
        }
    }

    // OnGUI function (MonoBehaviour).
    void OnGUI ()
    {
        if (GUI.Button (new Rect (0, 0, 300, 50), "Reset (startTime = 0)")) {
            ResetAndPlay (0.0f);
        }
        if (GUI.Button (new Rect (0, 50, 300, 50), "Reset (startTime = 1.3)")) {
            ResetAndPlay (1.3f);
        }
    }
}