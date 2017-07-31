using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserHazardSpawner : MonoBehaviour {

	public int Base;
	public int Step;
	public float BPM;
	public int startStep;
	public int CurrentStep = 1;
	public int CurrentMeasure = 1;
	private float interval;
	private float nextTime;
	public GameObject shifterCheck;
	public GameObject laser,marker;
	public Transform[] hazardPositions;
	public List<int> lasersteps,markerSteps;
	public List<int> measuresList;
	public int maxMeasures;
	public bool spawned;
	public List<int> trackIndex;
	public bool active;
	public bool isMarker;
	public int i ;
	AudioTrackShifter shifter;
	AudioSource src;
	private bool startMeter;
	public bool isTrackCheck;
	public bool cancelContinue;
    public bool addScreenShake;
    public float screenShakeAmt;
    CamShakeSimple camShake;

	// Use this for initialization
	void Start () {

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

	IEnumerator DoTick() // yield methods return IEnumerator
	{
		for (; ;) 

		{




			if (lasersteps.Contains (CurrentStep)&& active && measuresList.Contains(CurrentMeasure)) 
			{

				//if (i > hazardPositions.Length -1)
				//i = 0;



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
			CurrentStep++;
			if (CurrentStep > Step) {
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
