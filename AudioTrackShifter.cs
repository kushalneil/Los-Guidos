using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent (typeof (AudioSource))]

public class AudioTrackShifter : MonoBehaviour {
	

	public bool sectionShift;
	public GameObject CheckpointSystem;
	CheckpointSystem check;
	public int Base;
	public int Step;
	private int normalStep;
	public float BPM;
	public int CurrentStep = 1;
	public int CurrentMeasure;
	private float interval;
	private float nextTime;
	public int startStep;
	AudioSource src;
	public AudioClip[] tracks;
	public int trackIndex;
	public bool shift;
	private bool start;
	public List<int> noRepeat;
	public delegate void shifterDelegate();
	public shifterDelegate shiftEvent;
	public List<int> halfTrack;
	public int[] trackSteps;
	public int[] trackShiftIndex;
	public int[] sectionShiftIndex;
	public List<int> checkPointIndex;
	public bool superShift;






	void Start () {
		src = GetComponent<AudioSource> ();
		normalStep = Step;
		check = CheckpointSystem.GetComponent<CheckpointSystem> ();
		PlaceTrack ();
		StartMetronome ();

	}

	public void StartMetronome()
	{
		StopCoroutine("DoTick");
		CurrentStep = startStep; 
		var multiplier = Base / 4f;
		var tmpInterval = 60f / BPM;
		interval = tmpInterval / multiplier; 
		nextTime = Time.time; // set the relative time to now
		StartCoroutine("DoTick"); 
	}

	IEnumerator DoTick() // yield methods return IEnumerator
	{
		for (; ;) {

						
			if (CurrentStep == 1 && CurrentMeasure ==1) 
			
			{

				if (!start) 
				{
					src.Play ();
					TrackShift ();
					start = true;

				}



			}


			







			if (CurrentStep == 1 && shift &&!sectionShift) 
			{
				

				trackIndex++;
				src.clip = tracks [trackIndex];
				if (halfTrack.Contains (trackIndex))
					Step = Step / 2;
				else
					Step = trackSteps[trackIndex];
				src.Play ();
				if (noRepeat.Contains (trackIndex))
					shift = true;
				else
					shift = false;
				TrackShift ();


			}

			if (CurrentStep == 1 && sectionShift && shift) 
			{
				if (!superShift)
					trackIndex = trackShiftIndex [trackIndex];
				else 
				{
					trackIndex = sectionShiftIndex [trackIndex];
					superShift = false;
				}
				Step = trackSteps [trackIndex];
				src.clip = tracks [trackIndex];
				src.Play ();
				TrackShift ();
				Checkpoint ();


			}

					
			/*if (CurrentStep == 3 && trackIndex == 0)			//Turn on only during Day2
				shift = true;*/





			nextTime += interval; // add interval to our relative time
			yield return new WaitForSeconds (nextTime - Time.time); // wait for the difference delta between now and expected next time of hit
			CurrentStep++;
			if (CurrentStep > Step) {
				CurrentStep = 1;
				CurrentMeasure++;

			}
		}

	}


	public void SetShift()
	{



		shift = true;




	}

	public void TrackShift()
	{



		if (shiftEvent != null)
			shiftEvent ();



	}

	public void Checkpoint()
	{

		if (checkPointIndex.Contains (trackIndex)) 
		{

			check.trackIndex = trackIndex;


		}





	}

	public void PlaceTrack()
	{
		Debug.Log ("TrackPlaced");
		trackIndex = GameObject.FindWithTag ("Checkpoint").GetComponent<CheckpointSystem> ().trackIndex;
		src.clip = tracks [trackIndex];
		Step = trackSteps [trackIndex];
		startStep = trackSteps [trackIndex] - 4;

	}








}
