
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserHazardSpawner : MonoBehaviour {

	public int Base; // The denominator in the time signature ( for example 4/4, where 4 is the base )
	public int Step; // The numerator in the time signature ( for example 7/4, where 7 is the step )
	public float BPM; // The tempo of the track in beats per minute
	
	public int startStep; // The step where the metronome should start from
	public int CurrentStep = 1; // The current step 
	public int CurrentMeasure = 1; // The current measure or musical 'bar'
	private float interval; // The interval between two succsessive beats
	private float nextTime; // The time when the next beat should occur
	
	public GameObject shifterCheck; // This points to an Audio Track Shifter which chooses which part of the song we are in.
	public GameObject laser,marker; // Any hazard can be attached here as a gameobject. A marker can also be attached to show the player where the hazard will spawn.
	public Transform[] hazardPositions;// The local position where the hazards will be spawned
	public List<int> lasersteps,markerSteps;// The beat where a hazard or marker needs to be spawned
	public List<int> measuresList;// The bars where this script should be active
	public int maxMeasures;// The total number of bars for which the scrpit should be active 	 	
	public bool spawned;// To check if the hazards are spawned
	
	public List<int> trackIndex;// The track number for which the script should be active 
	public bool active;// To check if the script is active 
	public bool isMarker;// if isMarker is set to true the Hazard location will be 'marked' in game before it is spawned.
	
	private int i ;// loop counter
	
	AudioTrackShifter shifter;// A reference to the audio track shifter which is needed to check which track we are currently in.
	AudioSource src;// A reference to the audio source that is playing the track
	
	private bool startMeter; // A bool to check if the metronome is ticking
	public bool isTrackCheck; // If set  to true, the script will check if a track is needed to activate the script.
	public bool cancelContinue; //If set to true, the track will continue run after the Measures list has been exhausted.
        
	public bool addScreenShake; // A bool to add screenshake to the hazard
	public float screenShakeAmt;// A value for screenshake
	CamShakeSimple camShake; // A reference to the camera to call the a function for screenshake

	
	void Start () 
	{

	shifter = shifterCheck.GetComponent<AudioTrackShifter>();
	src = shifterCheck.GetComponent<AudioSource> ();
        if (addScreenShake)
        camShake = GameObject.FindWithTag("MainCamera").GetComponent<CamShakeSimple>();
    }

	public void StartMetronome()
	{
		
		StopCoroutine("DoTick");
		CurrentStep = 1;
		CurrentMeasure = 1;
		var multiplier = Base / 4f;
		var tmpInterval = 60f / BPM;
		interval = tmpInterval / multiplier; 
		nextTime = Time.time; // set the relative time to now
		StartCoroutine("DoTick"); 
	}

	IEnumerator DoTick() // All code to be executed at any 'beat' should be typed here.
	{
		for (; ;) 

		{
 



			if (lasersteps.Contains (CurrentStep)&& active && measuresList.Contains(CurrentMeasure)) 
			{

				for(int i=0;i<hazardPositions.Length;i++)	
				Instantiate (laser,hazardPositions[i].position,hazardPositions[i].rotation);
	                			
 				if(addScreenShake)
              			{
                    				camShake.BeginCamShake(screenShakeAmt);

               			}



			}

 			if (markerSteps.Contains (CurrentStep) && active && isMarker && measuresList.Contains (CurrentMeasure)) 
			{

				marker.SetActive (true);


			} 

			else if(isMarker)
				marker.SetActive (false);


			nextTime += interval; // add interval to our relative time
			yield return new WaitForSeconds (nextTime - Time.time); // wait for the difference delta between now and expected next time of hit
			CurrentStep++;			if (CurrentStep > Step) {
				CurrentStep = 1;
				CurrentMeasure++;
			
			}

			if (CurrentMeasure > maxMeasures) 

			{

				CurrentMeasure = 1;
				CurrentStep = 1;



			}
		}

	}



	public void Update()
	{
             // The script checks if the current track in the song is the activation track for this script
		if (trackIndex.Contains(shifter.trackIndex) && src.isPlaying && !startMeter) 
		{
			if(isTrackCheck)
			active = true;
			StartMetronome ();
			startMeter = true;
		}



		if (cancelContinue) 
		{	

			if (trackIndex.Contains (shifter.trackIndex)) 
			{

				active = true;
			} 

			else 
			{

				active = false;

			}
		}




	  }


}
