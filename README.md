smflite
=======

**smflite** is a minimal toolkit to handle standard MIDI files (SMF) on Unity.

What it is used for
-------------------

- Audio/visual app: Synchronize animations to pre-recorded audio clips.
- Rhythm games: Retrieve timing information from song data.

What it isn't used for
----------------------

- Playback MIDI songs.
- Retrieve meta-data or SysEx data from SMF.

How to use
----------

1. Load a MIDI file with **MidiFileLoader.Load**. It returns a **MidiFileContainer** instance which contains song data.

	MidiFileContainer song = MidiFileLoader.Load (smfAsset.bytes);

2. Create **MidiTrackSequencer** with song data. You can specify the BPM for playback here.

	seq = new MidiTrackSequencer (song.tracks[0], song.division, bpm);

3. Call the **Play** method in the sequencer class. It return a set of **MidiEvent** on the initial point of the song.

	foreach (MidiEvent e in seq.Start ()) {
	  // Do something with MidiEvent.
	}

4. Call the **Advance** method in every frame. You should give a delta time, and then it returns a set of MidiEvent which occurred between the previous frame and the current frame.

	void Update() {
	  foreach (MidiEvent e in seq.Advance (Time.deltaTime)) {
	    // Do something with MidiEvent.
	  }
	}

5. Run until **Playing** property become false.

You can see an example [here](https://github.com/keijiro/unity-smflite-test).

License
-------

Copyright (C) 2013 Keijiro Takahashi

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.



